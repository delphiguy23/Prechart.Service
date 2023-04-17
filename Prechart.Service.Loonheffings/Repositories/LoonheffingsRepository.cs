using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Globals.Models.Loonheffings;
using Prechart.Service.Globals.Models.Loonheffings.Xsd.Xsd2022;
using Prechart.Service.Loonheffings.Models;

namespace Prechart.Service.Loonheffings.Repositories;

public partial class LoonheffingsRepository : ILoonheffingsRepository
{
    private readonly IMongoCollection<XmlLoonaangifteUpload> _collection;
    private readonly ILogger<LoonheffingsRepository> _logger;

    public LoonheffingsRepository(ILogger<LoonheffingsRepository> logger,
        IMongoCollection<XmlLoonaangifteUpload> collection)
    {
        _logger = logger;
        _collection = collection;

        TryClassMapRegistration<InkomstenverhoudingInitieelTypeNatuurlijkPersoon>(typeof(InkomstenverhoudingInitieelTypeNatuurlijkPersoon));
        TryClassMapRegistration<Loonaangifte>(typeof(Loonaangifte));
        TryClassMapRegistration<LoonaangifteAdministratieveEenheid>(typeof(LoonaangifteAdministratieveEenheid));
        TryClassMapRegistration<TijdvakAangifteType>(typeof(TijdvakAangifteType));
        TryClassMapRegistration<TijdvakAangifteTypeVolledigeAangifte>(typeof(TijdvakAangifteTypeVolledigeAangifte));
        TryClassMapRegistration<AdresBinnenlandType>(typeof(AdresBinnenlandType));
        TryClassMapRegistration<AdresBuitenlandType>(typeof(AdresBuitenlandType));
        TryClassMapRegistration<NatuurlijkPersoonDetails>(typeof(NatuurlijkPersoonDetails));
    }

    public async Task<IFluentResults> HandleAsync(UpsertTaxFiling2022 request, CancellationToken cancellationToken = default)
    {
        try
        {
            var taxfile = request.TaxFiling;
            await _collection.InsertOneAsync(taxfile);
            return ResultsTo.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure().FromException(e);
        }
    }

    public async Task<IFluentResults> HandleAsync(GetUnProcessUploadedTaxFilingWithoutEmployees request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start flipping process bit of Taxfiles without Employees");

        try
        {
            var filters = Builders<XmlLoonaangifteUpload>.Filter.Exists
            (
                "Loonaangifte.AdministratieveEenheid.Items.Item.InkomstenverhoudingInitieel.NatuurlijkPersoon", false
            );

            filters &= Builders<XmlLoonaangifteUpload>.Filter.Eq(l => l.Processed, false);
            filters &= Builders<XmlLoonaangifteUpload>.Filter.Eq(l => l.IsValid, true);

            var update = Builders<XmlLoonaangifteUpload>.Update
                .Set(l => l.Processed, true)
                .Set(l => l.ProcessedDate, DateTime.Now)
                .Set(l => l.Errors, new List<string> {"No employees found"});

            var documents = await _collection.UpdateManyAsync(filters, update);
            _logger.LogInformation("End flipping process bit of Taxfiles without Employees");

            return ResultsTo.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure().FromException(e);
        }
    }

    public async Task<IFluentResults<List<UnprocessedUploads>>> HandleAsync(GetUnProcessUploadedTaxFiling request, CancellationToken cancellationToken = default)
    {
        var sw = new Stopwatch();
        sw.Start();
        try
        {
            _logger.LogInformation($"Start executing {nameof(GetUnProcessUploadedTaxFiling)}");

            var options = new AggregateOptions
            {
                AllowDiskUse = false,
            };

            var unprocessed = new List<UnprocessedUploads>();

            var pipelineNormalTaxFiles = UnprocessedUploadedTaxFilesPipelineDefinition();
            var unprocessedNormalTaxFiles = await _collection.Aggregate(pipelineNormalTaxFiles, options).ToListAsync();

            if (unprocessedNormalTaxFiles.Any())
            {
                unprocessedNormalTaxFiles.ForEach(t => t.Person?.ForEach(p => p.CollectieveType = CollectieveType.Normaal));
                unprocessed.AddRange(unprocessedNormalTaxFiles);
            }

            var pipelineCorrectionTaxFiles = UnprocessedUploadedTaxFilesCorrectionPipelineDefinition();
            var unprocessedCorrectionTaxFiles = await _collection.Aggregate(pipelineCorrectionTaxFiles, options).ToListAsync();

            if (unprocessedCorrectionTaxFiles.Any())
            {
                unprocessedCorrectionTaxFiles.ForEach(t => t.Person?.ForEach(p => p.CollectieveType = CollectieveType.Correctie));
                unprocessed.AddRange(unprocessedCorrectionTaxFiles);
            }

            _logger.LogInformation($"End executing {nameof(GetUnProcessUploadedTaxFiling)}");
            sw.Stop();
            _logger.LogInformation($"Time elapsed: {sw.Elapsed.TotalSeconds} s");
            return ResultsTo.Success(unprocessed);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            sw.Stop();
            _logger.LogInformation($"Time elapsed: {sw.Elapsed.TotalSeconds} s");
            return ResultsTo.Failure<List<UnprocessedUploads>>().FromException(e);
        }
    }

    public async Task<IFluentResults<UpdateResult>> HandleAsync(UpdateProcessedXml request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation($"Start executing {nameof(UpdateProcessedXml)}");

            var filters = Builders<XmlLoonaangifteUpload>.Filter.Eq(l => l.Processed, false);
            filters &= Builders<XmlLoonaangifteUpload>.Filter.Eq(l => l.IsValid, true);
            filters &= Builders<XmlLoonaangifteUpload>.Filter.Eq(l => l.FileName, request.FileName);

            var errors = new List<string>();

            if (!string.IsNullOrWhiteSpace(request.Errors))
            {
                errors.Add(request.Errors);
            }

            var update = Builders<XmlLoonaangifteUpload>.Update
                .Set(l => l.Processed, true)
                .Set(l => l.ProcessedDate, DateTime.Now)
                .Set(l => l.EmployeesUpdated, request.EmployeesUpdated)
                .Set(l => l.EmployeesInserted, request.EmployeesInserted)
                .Set(l => l.Errors, errors);

            var result = await _collection.UpdateManyAsync(filters, update);

            _logger.LogInformation($"End executing {nameof(UpdateProcessedXml)}");

            return ResultsTo.Success(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<UpdateResult>().FromException(e);
        }
    }

    private static PipelineDefinition<XmlLoonaangifteUpload, UnprocessedUploads> UnprocessedUploadedTaxFilesPipelineDefinition()
    {
        PipelineDefinition<XmlLoonaangifteUpload, UnprocessedUploads> pipeline = new[]
        {
            new("$match", new BsonDocument()
                .Add("Loonaangifte.AdministratieveEenheid.Items.Item.InkomstenverhoudingInitieel.NatuurlijkPersoon",
                    new BsonDocument()
                        .Add("$exists", new BsonBoolean(true))
                )
                .Add("Processed", new BsonBoolean(false))
                .Add("IsValid", new BsonBoolean(true))),
            new BsonDocument("$unwind", new BsonDocument()
                .Add("path", "$Loonaangifte.AdministratieveEenheid.Items")
                .Add("preserveNullAndEmptyArrays", new BsonBoolean(true))),
            new BsonDocument("$unwind", new BsonDocument()
                .Add("path", "$Loonaangifte.AdministratieveEenheid.Items.Item")
                .Add("preserveNullAndEmptyArrays", new BsonBoolean(true))),
            new BsonDocument("$project", new BsonDocument()
                .Add("FileName", "$FileName")
                .Add("TaxNo", "$Loonaangifte.AdministratieveEenheid.LhNr")
                .Add("Klant", "$Loonaangifte.AdministratieveEenheid.NmIP")
                .Add("TaxFileProcessDate", "$Loonaangifte.Bericht.DatTdAanm")
                .Add("PeriodStart", "$Loonaangifte.AdministratieveEenheid.Items.DatAanvTv")
                .Add("PeriodEnd", "$Loonaangifte.AdministratieveEenheid.Items.DatEindTv")
                .Add("CollectieveAangifteNormal", "$Loonaangifte.AdministratieveEenheid.Items.Item.CollectieveAangifte")
                .Add("Person", "$Loonaangifte.AdministratieveEenheid.Items.Item.InkomstenverhoudingInitieel")),
            new BsonDocument("$project", new BsonDocument()
                .Add("Person.DatAanv", 0.0)
                .Add("Person.DatEind", 0.0)
                .Add("Person.DatEindSpecified", 0.0)
                .Add("Person.CdRdnEindArbov", 0.0)
                .Add("Person.CdRdnEindArbovSpecified", 0.0)
                .Add("Person.Sector", 0.0)),
            new BsonDocument("$project", new BsonDocument()
                .Add("Person.NatuurlijkPersoon.GeslSpecified", 0.0)
                .Add("Person.Inkomstenperiode.CdAardSpecified", 0.0)
                .Add("Person.Inkomstenperiode.FsIndFZSpecified", 0.0)
                .Add("Person.Inkomstenperiode.IndArbovOnbepTdSpecified", 0.0)
                .Add("Person.Inkomstenperiode.IndSchriftArbovSpecified", 0.0)
                .Add("Person.Inkomstenperiode.IndOprovSpecified", 0.0)
                .Add("Person.Inkomstenperiode.IndJrurennrmSpecified", 0.0)
                .Add("Person.Inkomstenperiode.IndPubAanOnbepTdSpecified", 0.0)
                .Add("Person.Inkomstenperiode.IndAvrLkvOudrWnSpecified", 0.0)
                .Add("Person.Inkomstenperiode.IndAvrLkvDgBafSbSpecified", 0.0)
                .Add("Person.Inkomstenperiode.IndAvrLkvHpAgWnSpecified", 0.0)
                .Add("Person.Inkomstenperiode.CdRdnGnBijtSpecified", 0.0)
                .Add("Person.Inkomstenperiode.IndWgldOudReglSpecified", 0.0)
                .Add("Person.Inkomstenperiode.IndAvrLkvAgWnSpecified", 0.0)
                .Add("Person.Inkomstenperiode.IndVakBnSpecified", 0.0)
                .Add("Person.Inkomstenperiode.IndSA71Specified", 0.0)
                .Add("Person.Inkomstenperiode.IndSA72Specified", 0.0)
                .Add("Person.Inkomstenperiode.IndSA03Specified", 0.0)
                .Add("Person.Inkomstenperiode.CdIncInkVermSpecified", 0.0)
                .Add("Person.Werknemersgegevens.CtrctlnSpecified", 0.0)
                .Add("Person.Werknemersgegevens.AantCtrcturenPWkSpecified", 0.0)
                .Add("Person.Werknemersgegevens.BedrAlInWWBSpecified", 0.0)
                .Add("Person.Werknemersgegevens.BedrRchtAlSpecified", 0.0)),
        };
        return pipeline;
    }

    private static PipelineDefinition<XmlLoonaangifteUpload, UnprocessedUploads> UnprocessedUploadedTaxFilesCorrectionPipelineDefinition()
    {
        PipelineDefinition<XmlLoonaangifteUpload, UnprocessedUploads> pipeline = new[]
        {
            new("$match", new BsonDocument()
                .Add("Loonaangifte.AdministratieveEenheid.Items.InkomstenverhoudingInitieel.NatuurlijkPersoon",
                    new BsonDocument().Add("$exists", new BsonBoolean(true))
                )
                .Add("Processed", new BsonBoolean(false))
                .Add("IsValid", new BsonBoolean(true))),
            new BsonDocument("$unwind", new BsonDocument()
                .Add("path", "$Loonaangifte.AdministratieveEenheid.Items")
                .Add("preserveNullAndEmptyArrays", new BsonBoolean(true))),
            new BsonDocument("$project", new BsonDocument()
                .Add("FileName", "$FileName")
                .Add("TaxNo", "$Loonaangifte.AdministratieveEenheid.LhNr")
                .Add("Klant", "$Loonaangifte.AdministratieveEenheid.NmIP")
                .Add("TaxFileProcessDate", "$Loonaangifte.Bericht.DatTdAanm")
                .Add("PeriodStart", "$Loonaangifte.AdministratieveEenheid.Items.DatAanvTv")
                .Add("PeriodEnd", "$Loonaangifte.AdministratieveEenheid.Items.DatEindTv")
                .Add("CollectieveAangifteCorrection", "$Loonaangifte.AdministratieveEenheid.Items.CollectieveAangifte")
                .Add("Person", "$Loonaangifte.AdministratieveEenheid.Items.InkomstenverhoudingInitieel")),
            new BsonDocument("$project", new BsonDocument()
                .Add("Person.DatAanv", 0.0)
                .Add("Person.DatEind", 0.0)
                .Add("Person.DatEindSpecified", 0.0)
                .Add("Person.CdRdnEindArbov", 0.0)
                .Add("Person.CdRdnEindArbovSpecified", 0.0)
                .Add("Person.Sector", 0.0)),
            new BsonDocument("$project", new BsonDocument()
                .Add("Person.NatuurlijkPersoon.GeslSpecified", 0.0)
                .Add("Person.Inkomstenperiode.CdAardSpecified", 0.0)
                .Add("Person.Inkomstenperiode.FsIndFZSpecified", 0.0)
                .Add("Person.Inkomstenperiode.IndArbovOnbepTdSpecified", 0.0)
                .Add("Person.Inkomstenperiode.IndSchriftArbovSpecified", 0.0)
                .Add("Person.Inkomstenperiode.IndOprovSpecified", 0.0)
                .Add("Person.Inkomstenperiode.IndJrurennrmSpecified", 0.0)
                .Add("Person.Inkomstenperiode.IndPubAanOnbepTdSpecified", 0.0)
                .Add("Person.Inkomstenperiode.IndAvrLkvOudrWnSpecified", 0.0)
                .Add("Person.Inkomstenperiode.IndAvrLkvDgBafSbSpecified", 0.0)
                .Add("Person.Inkomstenperiode.IndAvrLkvHpAgWnSpecified", 0.0)
                .Add("Person.Inkomstenperiode.CdRdnGnBijtSpecified", 0.0)
                .Add("Person.Inkomstenperiode.IndWgldOudReglSpecified", 0.0)
                .Add("Person.Inkomstenperiode.IndAvrLkvAgWnSpecified", 0.0)
                .Add("Person.Inkomstenperiode.IndVakBnSpecified", 0.0)
                .Add("Person.Inkomstenperiode.IndSA71Specified", 0.0)
                .Add("Person.Inkomstenperiode.IndSA72Specified", 0.0)
                .Add("Person.Inkomstenperiode.IndSA03Specified", 0.0)
                .Add("Person.Inkomstenperiode.CdIncInkVermSpecified", 0.0)
                .Add("Person.Werknemersgegevens.CtrctlnSpecified", 0.0)
                .Add("Person.Werknemersgegevens.AantCtrcturenPWkSpecified", 0.0)
                .Add("Person.Werknemersgegevens.BedrAlInWWBSpecified", 0.0)
                .Add("Person.Werknemersgegevens.BedrRchtAlSpecified", 0.0)),
        };
        return pipeline;
    }

    private void TryClassMapRegistration<T>(Type param)
    {
        if (!BsonClassMap.IsClassMapRegistered(param))
        {
            try
            {
                BsonClassMap.RegisterClassMap<T>();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"{nameof(T)} is already registered.");
            }
        }
    }
}
