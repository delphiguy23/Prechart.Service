using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Globals.Helper;
using Prechart.Service.Globals.Models.Klant;
using Prechart.Service.Klant.Models;

namespace Prechart.Service.Klant.Repository;

public partial class KlantRepository : IKlantRepository
{
    private readonly IMongoCollection<KlantModel> _collection;
    private readonly ILogger<KlantRepository> _logger;
    private readonly IMapper _mapper;
    private readonly IMongoDbHelper _mongoDbHelper;

    public KlantRepository(
        ILogger<KlantRepository> logger,
        IMapper mapper,
        IMongoDbHelper mongoDbHelper,
        IMongoCollection<KlantModel> collection)
    {
        _logger = logger;
        _mapper = mapper;
        _mongoDbHelper = mongoDbHelper;
        _collection = collection;

        _mongoDbHelper.TryClassMapRegistration<KlantModel>(typeof(KlantModel));
        _mongoDbHelper.TryClassMapRegistration<ContactPersonModel>(typeof(ContactPersonModel));
        _mongoDbHelper.TryClassMapRegistration<ContactInfoModel>(typeof(ContactInfoModel));
        _mongoDbHelper.TryClassMapRegistration<PersonAddressModel>(typeof(PersonAddressModel));
        _mongoDbHelper.TryClassMapRegistration<AdresBinnenlandModel>(typeof(AdresBinnenlandModel));
        _mongoDbHelper.TryClassMapRegistration<AdresBuitenlandModel>(typeof(AdresBuitenlandModel));
    }

    public async Task<IFluentResults<List<KlantModel>>> HandleAsync(UpsertKlants request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Start executing {nameof(UpsertKlants)}");

        if (!request.Klants.Any())
        {
            return ResultsTo.NotFound<List<KlantModel>>().WithMessage("Argument is null or empty");
        }

        var klants = request.Klants;

        try
        {
            foreach (var klant in klants)
            {
                _logger.LogInformation("Upserting Klant Information {@KlantInfo}", klant);

                var klantFilter = Builders<KlantModel>.Filter.Eq(k => k.KlantNaam, klant.KlantNaam);
                klantFilter &= Builders<KlantModel>.Filter.Eq(k => k.Active, true);

                var found = await (await _collection.FindAsync(klantFilter)).FirstOrDefaultAsync();

                if (found is null)
                {
                    klant.Active = true;
                    klant.DateCreated = DateTime.Now;
                    klant.DateLastModified = DateTime.Now;

                    await _collection.InsertOneAsync(klant);
                }
                else
                {
                    klant.Id = found.Id;

                    foreach (var klantWerkgever in klant.Werkgevers)
                    {
                        if (!found.Werkgevers.Any(w => klant.Werkgevers.Contains(w)))
                        {
                            await _collection.FindOneAndUpdateAsync(k => k.Id == found.Id,
                                Builders<KlantModel>.Update.Push(w => w.Werkgevers, klantWerkgever));
                        }
                    }

                    var update = Builders<KlantModel>.Update
                        .Set(k => k.DateLastModified, DateTime.Now)
                        .Set(k => k.Active, klant.Active)
                        .Set(k => k.KlantNaam, klant.KlantNaam);

                    await _collection.UpdateOneAsync(Builders<KlantModel>.Filter.Eq(f => f.Id, found.Id), update);
                }
            }

            return ResultsTo.Success(klants);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<List<KlantModel>>().FromException(e);
        }
    }

    public async Task<IFluentResults<List<KlantModel>>> HandleAsync(GetKlants request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation($"Start executing {nameof(GetKlants)}");

            var result = await _collection.FindAsync(k => k.Active);

            if (result is null ||
                result?.Current is null ||
                !result.Any())
            {
                return ResultsTo.NotFound<List<KlantModel>>().WithMessage("No Clients found");
            }

            return ResultsTo.Success(await result.ToListAsync());
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<List<KlantModel>>().FromException(e);
        }
    }

    public async Task<IFluentResults<KlantModel>> HandleAsync(GetKlantById request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation($"Start executing {nameof(GetKlantById)}");

            var result = await _collection.FindAsync(k => k.Id == ObjectId.Parse(request.KlantId)
                                                          && k.Active);

            if (result is null ||
                result?.Current is null ||
                !result.Any())
            {
                return ResultsTo.NotFound<KlantModel>().WithMessage("No Client found");
            }

            return ResultsTo.Success(await result.FirstOrDefaultAsync());
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<KlantModel>().FromException(e);
        }
    }

    public async Task<IFluentResults<KlantModel>> HandleAsync(GetKlantByName request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation($"Start executing {nameof(GetKlantByName)}");

            var result = await _collection
                .FindAsync(k => k.KlantNaam.ToUpperInvariant() == request.KlantName.ToUpperInvariant()
                                && k.Active);

            if (result is null ||
                result?.Current is null ||
                !result.Any())
            {
                return ResultsTo.NotFound<KlantModel>().WithMessage("No Client found");
            }

            return ResultsTo.Success(await result.FirstOrDefaultAsync());
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<KlantModel>().FromException(e);
        }
    }

    public async Task<IFluentResults<KlantModel>> HandleAsync(GetKlantByTaxNo request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation($"Start executing {nameof(GetKlantByName)}");

            var result = await _collection.FindAsync(f => f.Werkgevers.Contains(request.TaxNo));

            if (result is null ||
                result?.Current is null ||
                !result.Any())
            {
                return ResultsTo.NotFound<KlantModel>().WithMessage("No Client found");
            }

            return ResultsTo.Success(await result.FirstOrDefaultAsync());
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<KlantModel>().FromException(e);
        }
    }
}
