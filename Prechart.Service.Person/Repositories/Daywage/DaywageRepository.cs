using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Prechart.Service.Core.Extensions;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Globals.Models.Loonheffings;
using Prechart.Service.Globals.Models.Person.Daywages;
using Prechart.Service.Person.Models;

namespace Prechart.Service.Person.Repositories.Daywage;

public partial class DaywageRepository : IDaywageRepository
{
    private readonly IMongoCollection<PersonModel> _collection;
    private readonly ILogger<DaywageRepository> _logger;

    public DaywageRepository(ILogger<DaywageRepository> logger,
        IMongoCollection<PersonModel> collection)
    {
        _logger = logger;
        _collection = collection;
    }

    public async Task<IFluentResults<List<TaxPaymentDetails>>> HandleAsync(GetTaxDetails request, CancellationToken cancellationToken = default)
    {
        var personFilter = Builders<PersonModel>.Filter.Eq(p => p.Id, request.PersonId.ToObjectId());
        personFilter &= Builders<PersonModel>.Filter.Eq(p => p.Active, true);

        var person = await (await _collection.FindAsync(personFilter)).FirstOrDefaultAsync(cancellationToken);

        if (person is null)
        {
            _logger.LogInformation($"Person with id {request.PersonId} not found");
            return ResultsTo.NotFound<List<TaxPaymentDetails>>().WithMessage($"Person with id {request.PersonId} not found");
        }

        var taxDetails = person.TaxPaymentDetails
            .Where(t => t.TaxNo == request.TaxNumber)
            .ToList();

        if (taxDetails is null || !taxDetails.Any())
        {
            _logger.LogInformation($"Tax details with tax number {request.TaxNumber} not found");
            return ResultsTo.NotFound<List<TaxPaymentDetails>>().WithMessage($"Tax details with tax number {request.TaxNumber} not found");
        }

        return ResultsTo.Something(taxDetails);
    }

    public async Task<IFluentResults> HandleAsync(UpsertDaywage request, CancellationToken cancellationToken = default)
    {
        var forUpsert = request.DaywageWithInReferencePeriode.DaywageRecord;
        var isInsert = forUpsert.DaywageId == ObjectId.Empty;
        forUpsert.DaywageId = isInsert ? ObjectId.GenerateNewId() : forUpsert.DaywageId;

        var personFilter = Builders<PersonModel>.Filter.Eq(s => s.Id, request.DaywageWithInReferencePeriode.PersonId.ToObjectId());
        UpdateDefinition<PersonModel> personUpdate;

        await _collection.FindOneAndUpdateAsync(p => p.Id == request.DaywageWithInReferencePeriode.PersonId.ToObjectId(),
            Builders<PersonModel>.Update.PullFilter(d => d.PersonDaywages,
                d => d.TaxNo == request.DaywageWithInReferencePeriode.DaywageRecord.TaxNo));

        personUpdate = Builders<PersonModel>.Update.Push(s => s.PersonDaywages, forUpsert);
        var status = await _collection.FindOneAndUpdateAsync(personFilter, personUpdate);

        return ResultsTo.Success();
    }

    public async Task<IFluentResults<List<PersonDaywageModel>>> HandleAsync(GetDaywage request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request?.PersonId) || string.IsNullOrWhiteSpace(request?.TaxNumber))
        {
            return ResultsTo.BadRequest<List<PersonDaywageModel>>().WithMessage("PersonId and TaxNumber are required");
        }

        var personFilter = Builders<PersonModel>.Filter.Eq(s => s.Id, request.PersonId.ToObjectId());
        personFilter &= Builders<PersonModel>.Filter.Eq(s => s.Active, true);

        var personDaywage = await _collection.FindAsync(personFilter).Result.FirstOrDefaultAsync();

        if (personDaywage?.PersonDaywages is null)
        {
            return ResultsTo.NotFound<List<PersonDaywageModel>>().WithMessage($"No daywage information for Person Id {request.PersonId} with Tax Number {request.TaxNumber}");
        }

        var daywageHistory = personDaywage.PersonDaywages
            .Where(p => p.TaxNo == request.TaxNumber)
            .Where(p => p.Active)
            .OrderByDescending(p => p.CalculatedOn)
            .ToList();

        return ResultsTo.Success(daywageHistory);
    }
}
