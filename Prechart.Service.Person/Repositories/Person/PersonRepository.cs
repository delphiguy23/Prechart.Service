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
using Prechart.Service.Core.Extensions;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Globals.Helper;
using Prechart.Service.Globals.Interfaces.Person;
using Prechart.Service.Globals.Models;
using Prechart.Service.Globals.Models.Klant;
using Prechart.Service.Globals.Models.Loonheffings.Xsd.Xsd2022;
using Prechart.Service.Globals.Models.Person;
using Prechart.Service.Person.Models;
using PersonModel = Prechart.Service.Person.Models.PersonModel;

namespace Prechart.Service.Person.Repositories.Person;

public partial class PersonRepository : IPersonRepository
{
    private readonly IBatchHelper _batchHelper;
    private readonly IMongoCollection<PersonModel> _collection;
    private readonly ILogger<PersonRepository> _logger;

    public PersonRepository(ILogger<PersonRepository> logger,
        IBatchHelper batchHelper,
        IMongoCollection<PersonModel> collection)
    {
        _logger = logger;
        _batchHelper = batchHelper;
        _collection = collection;

        if (!BsonClassMap.IsClassMapRegistered(typeof(InkomstenverhoudingInitieelTypeNatuurlijkPersoon)))
        {
            BsonClassMap.RegisterClassMap<InkomstenverhoudingInitieelTypeNatuurlijkPersoon>();
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(AdresBinnenlandType)))
        {
            BsonClassMap.RegisterClassMap<AdresBinnenlandType>();
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(AdresBuitenlandType)))
        {
            BsonClassMap.RegisterClassMap<AdresBuitenlandType>();
        }

        //
        if (!BsonClassMap.IsClassMapRegistered(typeof(KlantWerkgeverModel)))
        {
            BsonClassMap.RegisterClassMap<KlantWerkgeverModel>();
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(Globals.Models.Xsd.Loonheffings.AdresBinnenlandType)))
        {
            BsonClassMap.RegisterClassMap<Globals.Models.Xsd.Loonheffings.AdresBinnenlandType>();
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(Globals.Models.Xsd.Loonheffings.AdresBuitenlandType)))
        {
            BsonClassMap.RegisterClassMap<Globals.Models.Xsd.Loonheffings.AdresBuitenlandType>();
        }
    }

    public async Task<IFluentResults<List<PersonModel>>> HandleAsync(GetPersons request, CancellationToken cancellationToken = default)
    {
        var personList = await _collection.Find(p => p.Active).ToListAsync();

        var persons = request.PersonType switch
        {
            PersonType.Employee => personList.Where(p => p.PersonType == PersonType.Employee).ToList(),
            PersonType.Doctor => personList.Where(p => p.PersonType == PersonType.Doctor).ToList(),
            PersonType.CaseManager => personList.Where(p => p.PersonType == PersonType.CaseManager).ToList(),
            PersonType.KlantContactPerson => personList.Where(p => p.PersonType == PersonType.KlantContactPerson).ToList(),
            _ => personList,
        };

        return ResultsTo.Success(persons);
    }

    public async Task<IFluentResults<PersonModel>> HandleAsync(GetPersonById request, CancellationToken cancellationToken = default)
    {
        try
        {
            return ResultsTo.Success(await _collection.Find(p => p.Active && p.Id == ObjectId.Parse(request.Id)).FirstOrDefaultAsync());
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<PersonModel>().FromException(e);
        }
    }

    public async Task<IFluentResults<PersonModel>> HandleAsync(GetPersonByBsn request, CancellationToken cancellationToken = default)
    {
        try
        {
            return ResultsTo.Success(await _collection.Find(p => p.Active && p.SofiNr == request.Bsn).FirstOrDefaultAsync());
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<PersonModel>().FromException(e);
        }
    }

    public async Task<IFluentResults<PersonModel>> HandleAsync(GetPersonByName request, CancellationToken cancellationToken = default)
    {
        try
        {
            return ResultsTo.Success(await _collection.Find(p => p.Active && p.SignificantAchternaam == request.LastName).FirstOrDefaultAsync());
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<PersonModel>().FromException(e);
        }
    }

    public async Task<IFluentResults<PersonModel>> HandleAsync(GetPersonByUserName request, CancellationToken cancellationToken = default)
    {
        try
        {
            var person = await _collection.Find(p => p.Active && p.UserName.ToLowerInvariant() == request.UserName.ToLowerInvariant()).FirstOrDefaultAsync();
            return ResultsTo.Something(person);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<PersonModel>().FromException(e);
        }
    }

    public async Task<IFluentResults<PersonModel>> HandleAsync(GetPersonByCredential request, CancellationToken cancellationToken = default)
    {
        try
        {
            var options = new AggregateOptions
            {
                AllowDiskUse = false,
            };

            var aggregateResult = await _collection.Aggregate(PersonCredentialPipeline(), options).ToListAsync();
            var result = aggregateResult.FirstOrDefault(r =>
                r.SofiLast4 == request.LastBsn &&
                r.PersNr == request.Personnummer &&
                (
                    r.Geboortedatum.Date == request.Birthdate.Date ||
                    r.Geboortedatum.Date == request.Birthdate.ToUniversalTime().Date
                )
            );

            if (result is null)
            {
                return ResultsTo.NotFound<PersonModel>().WithMessage("Person not found");
            }

            return ResultsTo.Success(await _collection.Find(x => x.Id == result.Id).FirstOrDefaultAsync());
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<PersonModel>().FromException(e);
        }
    }

    public async Task<IFluentResults<PersonModel>> HandleAsync(DeletePerson request, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _collection.FindOneAndUpdateAsync
            (
                Builders<PersonModel>.Filter.Eq(d => d.Id, ObjectId.Parse(request.Id)),
                Builders<PersonModel>.Update.Set("Active", false)
            );

            if (result is null)
            {
                return ResultsTo.NotFound<PersonModel>().WithMessage("Person not found");
            }

            return ResultsTo.Success<PersonModel>();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<PersonModel>().FromException(e);
        }
    }

    public async Task<IFluentResults<List<PersonModel>>> HandleAsync(WerkgeversPersons request, CancellationToken cancellationToken = default)
    {
        try
        {
            var filter = Builders<PersonModel>
                .Filter
                .ElemMatch(p => p.Werkgever,
                    Builders<KlantWerkgeverModel>
                        .Filter
                        .Eq(k => k.LoonheffingsNr, request.TaxNo)
                );
            filter &= Builders<PersonModel>.Filter.Eq(p => p.Active, true);
            filter &= Builders<PersonModel>.Filter.Eq(p => p.PersonType, PersonType.Employee);

            var result = await _collection.Find(filter).ToListAsync();

            foreach (var record in result)
            {
                var orderedTaxPaymentDetails = record.TaxPaymentDetails.OrderByDescending(x => x.TaxPeriode).ToList();
                record.TaxPaymentDetails = orderedTaxPaymentDetails;
            }

            return ResultsTo.Success(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<List<PersonModel>>().FromException(e);
        }
    }

    public async Task<IFluentResults<PersonUpsertResults>> HandleAsync(UpsertPersons request, CancellationToken cancellationToken = default)
    {
        var _sw = new Stopwatch();
        _sw.Start();

        try
        {
            var retry = _batchHelper.AwaitAndRetry<List<PersonModel>>(5);

            var upserted = new List<IPersonUpsertResult>();

            _logger.LogInformation("Performing bulk insert");
            var bulkInsertResult = await PerformBulkInsert(request);

            var sofiForUpdateFilter = Builders<PersonModel>.Filter.In(s => s.SofiNr, bulkInsertResult.ForUpsert.Select(p => p.SofiNr).ToList());
            sofiForUpdateFilter &= Builders<PersonModel>.Filter.Eq(s => s.Active, true);

            _logger.LogInformation("Determining items for update");
            var foundForUpdate = await retry.ExecuteAsync(async () => await _collection.FindAsync(sofiForUpdateFilter).Result.ToListAsync());

            var totalForUpdate = bulkInsertResult.ForUpsert.Count;
            var currentForUpdate = 0;

            var tasks = new List<Task>();

            _logger.LogInformation("Building update objects");
            foreach (var person in bulkInsertResult.ForUpsert)
            {
                _logger.LogInformation($"Processing {currentForUpdate++} of {totalForUpdate}");
                var found = foundForUpdate.FirstOrDefault(p => p.SofiNr == person.SofiNr);

                if (found is null)
                {
                    continue;
                }

                tasks.Add(DoUpdates(person, found));
            }

            var sw = new Stopwatch();
            sw.Start();

            await Task.WhenAll(tasks);

            foreach (var task in tasks)
            {
                var taskTesult = ((Task<PersonUpsertResult>) task).Result;
                upserted.Add(taskTesult);
            }

            sw.Stop();
            _logger.LogInformation($"Elapsed time: {sw.Elapsed.TotalSeconds} s");

            upserted.AddRange(bulkInsertResult.Upserted);
            var result = new PersonUpsertResults {Results = upserted};

            _sw.Stop();
            _logger.LogInformation($"Overall Elapsed time: {_sw.Elapsed.TotalSeconds} s");
            return ResultsTo.Success(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            _sw.Stop();
            _logger.LogInformation($"Overall Elapsed time: {_sw.Elapsed.TotalSeconds} s");
            return ResultsTo.Failure<PersonUpsertResults>().FromException(e);
        }
    }

    public async Task<IFluentResults<bool>> HandleAsync(UpdatePersonPhoto request, CancellationToken cancellationToken)
    {
        try
        {
            var getPersonUser = Builders<PersonModel>.Filter.Eq(w => w.Id, request.Id);
            getPersonUser &= Builders<PersonModel>.Filter.Eq(w => w.Active, true);

            var personUser = await _collection.FindAsync(getPersonUser).Result.FirstOrDefaultAsync();

            if (personUser is null)
            {
                return ResultsTo.BadRequest<bool>("Person does not exist");
            }

            var updated = await _collection.FindOneAndUpdateAsync(
                Builders<PersonModel>.Filter.Eq(w => w.Id, personUser.Id),
                Builders<PersonModel>.Update
                    .Set(w => w.PersonPhoto, request.PersonPhoto)
                    .Set(w => w.DateLastModified, DateTime.Now));

            return ResultsTo.Success(updated is not null);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<bool>().FromException(e);
        }
    }

    public async Task<IFluentResults<PersonPhotoModel>> HandleAsync(DownloadPersonPhoto request, CancellationToken cancellationToken)
    {
        try
        {
            var getPersonUser = Builders<PersonModel>.Filter.Eq(w => w.Id, request.Id);
            getPersonUser &= Builders<PersonModel>.Filter.Eq(w => w.Active, true);

            var personUser = await _collection.FindAsync(getPersonUser).Result.FirstOrDefaultAsync();

            if (personUser is null || personUser?.PersonPhoto is null)
            {
                return ResultsTo.BadRequest<PersonPhotoModel>("Person does not exist or Person didn't Upload Photo");
            }

            return ResultsTo.Success(personUser.PersonPhoto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<PersonPhotoModel>().FromException(e);
        }
    }

    public async Task<IFluentResults<bool>> HandleAsync(UpsertPersonUserCredentials request, CancellationToken cancellationToken = default)
    {
        try
        {
            var updated = await _collection.FindOneAndUpdateAsync(
                Builders<PersonModel>.Filter.Eq(w => w.Id, request.PersonId.ToObjectId()),
                Builders<PersonModel>.Update
                    .Set(w => w.UserName, request.UserName)
                    .Set(w => w.EmailAddress, request.EmailAddress)
            );
            return ResultsTo.Something(updated is not null);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<bool>().FromException(e);
        }
    }

    public async Task<IFluentResults<PersonUserModel>> HandleAsync(UpsertPersonUser request, CancellationToken cancellationToken = default)
    {
        try
        {
            var insertPerson = new PersonModel();
            var getPersonUser = Builders<PersonModel>.Filter.Eq(w => w.Id, request.PersonUser.Id.ToObjectId());
            getPersonUser &= Builders<PersonModel>.Filter.Eq(w => w.Active, true);

            var personUser = await _collection.FindAsync(getPersonUser).Result.FirstOrDefaultAsync();

            if (personUser is null)
            {
                return ResultsTo.NotFound<PersonUserModel>().WithMessage("Person does not exists");
            }

            await _collection.FindOneAndUpdateAsync(
                Builders<PersonModel>.Filter.Eq(w => w.Id, personUser.Id),
                Builders<PersonModel>.Update
                    .Set(w => w.Voorletter, request.PersonUser.Voorletter)
                    .Set(w => w.Voorvoegsel, request.PersonUser.Voorvoegsel)
                    .Set(w => w.SignificantAchternaam, request.PersonUser.SignificantAchternaam)
                    .Set(w => w.Geboortedatum, request.PersonUser.Geboortedatum.ToUniversalTime())
                    .Set(w => w.Nationaliteit, request.PersonUser.Nationaliteit)
                    .Set(w => w.Geslacht, request.PersonUser.Geslacht)
                    .Set(w => w.PersonType, request.PersonUser.PersonType)
                    .Set(w => w.AdresBinnenland, request.PersonUser.AdresBinnenland)
                    .Set(w => w.AdresBuitenland, request.PersonUser.AdresBuitenland)
                    .Set(w => w.DateLastModified, DateTime.Now));

            var getPersonUserModified = Builders<PersonModel>.Filter.Eq(w => w.Id, personUser.Id);
            var upsertPerson = await _collection.FindAsync(getPersonUserModified).Result.FirstOrDefaultAsync();

            var result = new PersonUserModel
            {
                Id = upsertPerson.Id.ToString(),
                AdresBinnenland = upsertPerson.AdresBinnenland,
                AdresBuitenland = upsertPerson.AdresBuitenland,
                Geboortedatum = upsertPerson.Geboortedatum.ToLocalTime(),
                Geslacht = upsertPerson.Geslacht,
                Nationaliteit = upsertPerson.Nationaliteit,
                PersonType = upsertPerson.PersonType,
                SignificantAchternaam = upsertPerson.SignificantAchternaam,
                SofiNr = upsertPerson.SofiNr,
                Voorletter = upsertPerson.Voorletter,
                Voorvoegsel = upsertPerson.Voorvoegsel,
                Active = upsertPerson.Active,
            };

            return ResultsTo.Success(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<PersonUserModel>().FromException(e);
        }
    }

    public async Task<IFluentResults<PersonUserModel>> HandleAsync(GetProfile request, CancellationToken cancellationToken = default)
    {
        try
        {
            var getProfileFilter = Builders<PersonModel>.Filter.Eq(w => w.Id, request.Id);
            getProfileFilter &= Builders<PersonModel>.Filter.Eq(w => w.Active, true);

            var personModel = await _collection.FindAsync(getProfileFilter).Result.FirstOrDefaultAsync();

            if (personModel is null)
            {
                return ResultsTo.NotFound<PersonUserModel>().WithMessage("Person does not exists");
            }

            var profile = new PersonUserModel
            {
                Id = personModel.Id.ToString(),
                AdresBinnenland = personModel.AdresBinnenland,
                AdresBuitenland = personModel?.AdresBuitenland,
                Geboortedatum = personModel.Geboortedatum.ToLocalTime(),
                Geslacht = personModel.Geslacht,
                Nationaliteit = personModel.Nationaliteit,
                PersonType = personModel.PersonType,
                SignificantAchternaam = personModel.SignificantAchternaam,
                SofiNr = personModel.SofiNr,
                Voorletter = personModel.Voorletter,
                Voorvoegsel = personModel.Voorvoegsel,
                Active = personModel.Active,
            };

            return ResultsTo.Success(profile);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<PersonUserModel>().FromException(e);
        }
    }

    public async Task<IFluentResults<PersonUserModel>> HandleAsync(DeactivatePersonUser request, CancellationToken cancellationToken = default)
    {
        try
        {
            var updatePerson = new PersonModel();
            var getPersonUser = Builders<PersonModel>.Filter.Eq(w => w.Id, request.Id);
            getPersonUser &= Builders<PersonModel>.Filter.Eq(w => w.Active, true);

            var personUser = await _collection.FindAsync(getPersonUser).Result.FirstOrDefaultAsync();
            if (personUser is null)
            {
                return ResultsTo.NotFound<PersonUserModel>().WithMessage("Person does not exists");
            }

            await _collection.FindOneAndUpdateAsync(
                Builders<PersonModel>.Filter.Eq(w => w.Id, personUser.Id),
                Builders<PersonModel>.Update
                    .Set(w => w.Active, false)
                    .Set(w => w.DateLastModified, DateTime.Now));

            var getPersonUserModified = Builders<PersonModel>.Filter.Eq(w => w.Id, request.Id);
            var personProfile = await _collection.FindAsync(getPersonUserModified).Result.FirstOrDefaultAsync();

            var result = new PersonUserModel
            {
                Id = personProfile.Id.ToString(),
                AdresBinnenland = personProfile.AdresBinnenland,
                AdresBuitenland = personProfile?.AdresBuitenland,
                Geboortedatum = personProfile.Geboortedatum.ToLocalTime(),
                Geslacht = personProfile.Geslacht,
                Nationaliteit = personProfile.Nationaliteit,
                PersonType = personProfile.PersonType,
                SignificantAchternaam = personProfile.SignificantAchternaam,
                SofiNr = personProfile.SofiNr,
                Voorletter = personProfile.Voorletter,
                Voorvoegsel = personProfile.Voorvoegsel,
                Active = personProfile.Active,
            };

            return ResultsTo.Success(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<PersonUserModel>().FromException(e);
        }
    }


    public async Task<IFluentResults<PersonTaxCumulative>> HandleAsync(GetPersonTaxCumulative request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start determining cumulatives for {RequestBsn}", request.Bsn);

        try
        {
            var options = new AggregateOptions
            {
                AllowDiskUse = false,
            };

            var pipelinePersonTaxCumulative = PersonTaxCumulativePipelineDefinition(request.Bsn);
            var aggregateResult = await _collection.Aggregate(pipelinePersonTaxCumulative, options).ToListAsync();

            _logger.LogInformation("Done determining cumulatives for {RequestBsn}", request.Bsn);

            return ResultsTo.Something(aggregateResult.GetCumulative<List<PersonTaxCumulative>>());
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<PersonTaxCumulative>().FromException(e);
        }
    }

    public async Task<IFluentResults<List<PersonModel>>> HandleAsync(GetPersonsByBsn request, CancellationToken cancellationToken = default)
    {
        try
        {
            return ResultsTo.Something(await _collection.Find(p => p.Active && request.Bsn.Contains(p.SofiNr)).ToListAsync());
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<List<PersonModel>>().FromException(e);
        }
    }

    private static PipelineDefinition<PersonModel, PersonTaxCumulative> PersonTaxCumulativePipelineDefinition(string bsn)
    {
        PipelineDefinition<PersonModel, PersonTaxCumulative> pipeline = new[]
        {
            new BsonDocument("$match", new BsonDocument()
                .Add("SofiNr", bsn)
                .Add("Active", new BsonBoolean(true))),
            new BsonDocument("$project", new BsonDocument()
                .Add("SofiNr", "$SofiNr")
                .Add("TaxPaymentDetails", "$TaxPaymentDetails")),
            new BsonDocument("$unwind", new BsonDocument()
                .Add("path", "$TaxPaymentDetails")),
            new("$project", new BsonDocument()
                .Add("_id", 0.0)
                .Add("SofiNr", "$SofiNr")
                .Add("PersonNr", "$TaxPaymentDetails.PersonNr")
                .Add("NumIv", "$TaxPaymentDetails.NumIv")
                .Add("TaxNo", "$TaxPaymentDetails.TaxNo")
                .Add("TaxFileProcessDate", "$TaxPaymentDetails.TaxFileProcessDate")
                .Add("TaxPeriode", "$TaxPaymentDetails.TaxPeriode")
                .Add("LnLbPh", "$TaxPaymentDetails.Werknemersgegevens.LnLbPh")
                .Add("LnSv", "$TaxPaymentDetails.Werknemersgegevens.LnSv")
                .Add("PrlnAofAnwLg", "$TaxPaymentDetails.Werknemersgegevens.PrlnAofAnwLg")
                .Add("PrlnAofAnwHg", "$TaxPaymentDetails.Werknemersgegevens.PrlnAofAnwHg")
                .Add("PrlnAofAnwUit", "$TaxPaymentDetails.Werknemersgegevens.PrlnAofAnwUit")
                .Add("PrlnAwfAnwLg", "$TaxPaymentDetails.Werknemersgegevens.PrlnAwfAnwLg")
                .Add("PrlnAwfAnwHg", "$TaxPaymentDetails.Werknemersgegevens.PrlnAwfAnwHg")
                .Add("PrlnAwfAnwHz", "$TaxPaymentDetails.Werknemersgegevens.PrlnAwfAnwHz")
                .Add("PrLnUfo", "$TaxPaymentDetails.Werknemersgegevens.PrLnUfo")
                .Add("LnTabBb", "$TaxPaymentDetails.Werknemersgegevens.LnTabBb")
                .Add("VakBsl", "$TaxPaymentDetails.Werknemersgegevens.VakBsl")
                .Add("OpgRchtVakBsl", "$TaxPaymentDetails.Werknemersgegevens.OpgRchtVakBsl")
                .Add("OpnAvwb", "$TaxPaymentDetails.Werknemersgegevens.OpnAvwb")
                .Add("OpbAvwb", "$TaxPaymentDetails.Werknemersgegevens.OpbAvwb")
                .Add("LnInGld", "$TaxPaymentDetails.Werknemersgegevens.LnInGld")
                .Add("WrdLn", "$TaxPaymentDetails.Werknemersgegevens.WrdLn")
                .Add("LnOwrk", "$TaxPaymentDetails.Werknemersgegevens.LnOwrk")
                .Add("VerstrAanv", "$TaxPaymentDetails.Werknemersgegevens.VerstrAanv")
                .Add("IngLbPh", "$TaxPaymentDetails.Werknemersgegevens.IngLbPh")
                .Add("PrAofLg", "$TaxPaymentDetails.Werknemersgegevens.PrAofLg")
                .Add("PrAofHg", "$TaxPaymentDetails.Werknemersgegevens.PrAofHg")
                .Add("PrAofUit", "$TaxPaymentDetails.Werknemersgegevens.PrAofUit")
                .Add("OpslWko", "$TaxPaymentDetails.Werknemersgegevens.OpslWko")
                .Add("PrGediffWhk", "$TaxPaymentDetails.Werknemersgegevens.PrGediffWhk")
                .Add("PrAwfLg", "$TaxPaymentDetails.Werknemersgegevens.PrAwfLg")
                .Add("PrAwfHg", "$TaxPaymentDetails.Werknemersgegevens.PrAwfHg")
                .Add("PrAwfHz", "$TaxPaymentDetails.Werknemersgegevens.PrAwfHz")
                .Add("PrUfo", "$TaxPaymentDetails.Werknemersgegevens.PrUfo")
                .Add("BijdrZvw", "$TaxPaymentDetails.Werknemersgegevens.BijdrZvw")
                .Add("WghZvw", "$TaxPaymentDetails.Werknemersgegevens.WghZvw")
                .Add("WrdPrGebrAut", "$TaxPaymentDetails.Werknemersgegevens.WrdPrGebrAut")
                .Add("WrknBijdrAut", "$TaxPaymentDetails.Werknemersgegevens.WrknBijdrAut")
                .Add("Reisk", "$TaxPaymentDetails.Werknemersgegevens.Reisk")
                .Add("VerrArbKrt", "$TaxPaymentDetails.Werknemersgegevens.VerrArbKrt")
                .Add("AantVerlU", "$TaxPaymentDetails.Werknemersgegevens.AantVerlU")
                .Add("Ctrctln", "$TaxPaymentDetails.Werknemersgegevens.Ctrctln")
                .Add("AantCtrcturenPWk", "$TaxPaymentDetails.Werknemersgegevens.AantCtrcturenPWk")
                .Add("BedrRntKstvPersl", "$TaxPaymentDetails.Werknemersgegevens.BedrRntKstvPersl")
                .Add("BedrAlInWwb", "$TaxPaymentDetails.Werknemersgegevens.BedrAlInWwb")
                .Add("BedrRchtAl", "$TaxPaymentDetails.Werknemersgegevens.BedrRchtAl")),
        };

        return pipeline;
    }

    private async Task<PersonUpsertResult> DoUpdates(PersonModel person, PersonModel found)
    {
        person.DateLastModified = DateTime.Now;
        person.DateCreated = found.DateCreated;
        person.Id = found.Id;
        person.Active = found.Active;

        var upsertWerkgever = UpsertPersonWerkgever(person, found);
        var upsertTaxDetails = UpsertPersonTaxDetails(person, found);

        if (string.IsNullOrEmpty(person.UserName))
        {
            var update = PersonUpdateBuilder(person);

            var retry = _batchHelper.AwaitAndRetry<UpdateResult>(5);

            await retry.ExecuteAsync(async () => await _collection.UpdateOneAsync(Builders<PersonModel>.Filter.Eq(s => s.Id, person.Id), update));
        }
        else
        {
            _logger.LogInformation("Skipped update of person record");
        }

        await Task.WhenAll(upsertWerkgever, upsertTaxDetails);

        return new PersonUpsertResult
        {
            Id = person.Id.ToString(),
            Bsn = person.SofiNr,
            Operation = DataOperation.Update,
            TaxFileName = person.TaxFileName,
        };
    }

    private async Task<(List<PersonModel> ForUpsert, List<IPersonUpsertResult> Upserted)> PerformBulkInsert(UpsertPersons request)
    {
        var persons = request.Persons;

        var bsnFilter = Builders<PersonModel>.Filter.In(s => s.SofiNr, persons.Select(p => p.SofiNr).ToList());
        bsnFilter &= Builders<PersonModel>.Filter.Eq(s => s.Active, true);

        var personsFound = await _collection.Find(bsnFilter).ToListAsync();

        var notFoundBsns = persons.Select(p => p.SofiNr).Except(personsFound.Select(p => p.SofiNr)).ToList();

        var forInsert = new List<PersonModel>();

        foreach (var notFoundBsn in notFoundBsns)
        {
            var firstBsnOccurence =
                persons.Where(p => p.SofiNr == notFoundBsn).OrderBy(p => p.TaxFileName).FirstOrDefault();

            if (firstBsnOccurence is null)
            {
                continue;
            }

            firstBsnOccurence.Active = true;
            firstBsnOccurence.DateCreated = DateTime.Now;
            firstBsnOccurence.DateLastModified = DateTime.Now;

            forInsert.Add(firstBsnOccurence);
            persons.Remove(firstBsnOccurence);
        }

        var upserted = new List<IPersonUpsertResult>();

        if (forInsert.Any())
        {
            await _collection.InsertManyAsync(forInsert);

            foreach (var inserted in forInsert)
            {
                upserted.Add(new PersonUpsertResult
                {
                    Id = inserted.Id.ToString(),
                    Bsn = inserted.SofiNr,
                    Operation = DataOperation.Insert,
                    TaxFileName = inserted.TaxFileName,
                });
            }
        }

        return (ForUpsert: persons, Upserted: upserted);
    }

    private async Task UpsertPersonTaxDetails(PersonModel person, PersonModel found)
    {
        var retry = _batchHelper.AwaitAndRetry<PersonModel>(5);

        foreach (var detail in person.TaxPaymentDetails)
        {
            if (found?.TaxPaymentDetails is null || !found.TaxPaymentDetails.Any(d =>
                    d.TaxNo == detail.TaxNo &&
                    d.PersonNr == detail.PersonNr &&
                    d.NumIv == detail.NumIv &&
                    d.TaxPeriode == detail.TaxPeriode &&
                    d.TaxFileProcessDate == detail.TaxFileProcessDate
                ))
            {
                await retry.ExecuteAsync(async () => await _collection.FindOneAndUpdateAsync(p => p.Id == person.Id, Builders<PersonModel>.Update.Push(w => w.TaxPaymentDetails, detail)));
            }
        }
    }

    private async Task UpsertPersonWerkgever(PersonModel person, PersonModel found)
    {
        var retry = _batchHelper.AwaitAndRetry<PersonModel>(5);

        foreach (var werkgever in person.Werkgever)
        {
            if (found?.Werkgever is not null &&
                !found.Werkgever.Any(w => w.LoonheffingsNr == werkgever.LoonheffingsNr))
            {
                await retry.ExecuteAsync(async () => await _collection.FindOneAndUpdateAsync
                    (p => p.Id == person.Id, Builders<PersonModel>.Update.Push(w => w.Werkgever, werkgever)));
            }
        }
    }

    private static UpdateDefinition<PersonModel> PersonUpdateBuilder(PersonModel person)
    {
        return Builders<PersonModel>.Update
            .Set(l => l.SofiNr, person.SofiNr)
            .Set(l => l.Voorletter, person.Voorletter)
            .Set(l => l.Voorvoegsel, person.Voorvoegsel)
            .Set(l => l.SignificantAchternaam, person.SignificantAchternaam)
            .Set(l => l.Geboortedatum, person.Geboortedatum)
            .Set(l => l.Nationaliteit, person.Nationaliteit)
            .Set(l => l.Geslacht, person.Geslacht)
            .Set(l => l.AdresBinnenland, person.AdresBinnenland)
            .Set(l => l.AdresBuitenland, person.AdresBuitenland)
            .Set(l => l.Active, person.Active)
            .Set(l => l.DateCreated, person.DateCreated)
            .Set(l => l.DateLastModified, person.DateLastModified);
    }

    private static PipelineDefinition<PersonModel, PersonCredentialsModel> PersonCredentialPipeline()
    {
        PipelineDefinition<PersonModel, PersonCredentialsModel> pipeline = new[]
        {
            new("$match", new BsonDocument()
                .Add("Active", new BsonBoolean(true))),
            new BsonDocument("$project", new BsonDocument()
                .Add("SofiNr", "$SofiNr")
                .Add("SofiLast4", new BsonDocument()
                    .Add("$substr", new BsonArray()
                        .Add("$SofiNr")
                        .Add(5.0)
                        .Add(4.0)
                    )
                )
                .Add("TaxPaymentDetails", "$TaxPaymentDetails")
                .Add("Geboortedatum", "$Geboortedatum")),
            new BsonDocument("$unwind", new BsonDocument()
                .Add("path", "$TaxPaymentDetails")),
            new BsonDocument("$project", new BsonDocument()
                .Add("_id", "$_id")
                .Add("SofiNr", "$SofiNr")
                .Add("SofiLast4", "$SofiLast4")
                .Add("PersNr", "$TaxPaymentDetails.PersonNr")
                .Add("Geboortedatum", "$Geboortedatum")),
        };

        return pipeline;
    }
}
