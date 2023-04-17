using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Globals.Helper;
using Prechart.Service.Globals.Models.Loonheffings;
using Prechart.Service.Globals.Models.Werkgever;
using Prechart.Service.Werkgever.Database.Context;
using Prechart.Service.Werkgever.Database.Models;

namespace Prechart.Service.Werkgever.Repository;

public partial class WerkgeverRepository : IWerkgeverRepository
{
    private readonly IMongoCollection<MongoWerkgeverModel> _collection;
    private readonly IWerkgeverDbContext _context;
    private readonly ILogger<WerkgeverRepository> _logger;
    private readonly IMapper _mapper;
    private readonly IMongoDbHelper _mongoDbHelper;

    public WerkgeverRepository(ILogger<WerkgeverRepository> logger, IWerkgeverDbContext context, IMapper mapper,
        IMongoCollection<MongoWerkgeverModel> collection, IMongoDbHelper mongoDbHelper
    )
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
        _collection = collection;
        _mongoDbHelper = mongoDbHelper;

        _mongoDbHelper.TryClassMapRegistration<MongoWerkgeverModel>(typeof(MongoWerkgeverModel));
        _mongoDbHelper.TryClassMapRegistration<MongoWhkPremie>(typeof(MongoWhkPremie));
        _mongoDbHelper.TryClassMapRegistration<WerkgeverKlantModel>(typeof(WerkgeverKlantModel));
    }

    public async Task<IFluentResults<List<MongoWerkgeverModel>>> HandleAsync(UpsertWerkgevers request, CancellationToken cancellationToken = default)
    {
        try
        {
            var werkgevers = request.Werkgevers;

            foreach (var werkgever in werkgevers)
            {
                var taxfilter = Builders<MongoWerkgeverModel>.Filter.Eq(w => w.FiscaalNummer, werkgever.FiscaalNummer);
                taxfilter &= Builders<MongoWerkgeverModel>.Filter.Eq(w => w.Actief, true);

                var found = await _collection.FindAsync(taxfilter).Result.FirstOrDefaultAsync();

                if (found is null)
                {
                    werkgever.Actief = true;
                    werkgever.DateCreated = DateTime.Now;
                    werkgever.DateLastModified = DateTime.Now;

                    if (werkgever.WhkPremies is not null)
                    {
                        werkgever.WhkPremies.ForEach(w =>
                        {
                            w.Id = ObjectId.GenerateNewId();
                            w.DateCreated = DateTime.Now;
                            w.DateLastModified = DateTime.Now;
                        });
                    }

                    await _collection.InsertOneAsync(werkgever);
                }
                else
                {
                    werkgever.DateLastModified = DateTime.Now;
                    werkgever.Id = found.Id;

                    if (werkgever?.WhkPremies is not null)
                    {
                        foreach (var whk in werkgever?.WhkPremies)
                        {
                            var whkFound = found?.WhkPremies?.FirstOrDefault(p => (p.Id == whk.Id || p.SqlId == whk.SqlId) && p.Actief);

                            whk.Id = whkFound?.Id ?? ObjectId.GenerateNewId();
                            whk.DateCreated = whkFound?.DateCreated ?? DateTime.Now;
                            whk.DateLastModified = DateTime.Now;
                            whk.SqlId = whkFound?.SqlId ?? 0;

                            await _collection.FindOneAndUpdateAsync(w => w.Id == werkgever.Id,
                                Builders<MongoWerkgeverModel>.Update.PullFilter(w => w.WhkPremies,
                                    f => f.Id == whk.Id));

                            await _collection.FindOneAndUpdateAsync(w => w.Id == werkgever.Id,
                                Builders<MongoWerkgeverModel>.Update.Push(w => w.WhkPremies, whk));
                        }
                    }

                    if (werkgever?.Collectieve is not null)
                    {
                        foreach (var collectieve in werkgever.Collectieve)
                        {
                            var collectieveFound = found?.Collectieve?
                                .OrderByDescending(p => p.ProcessedDate)
                                .ThenByDescending(p => p.Periode.Trim())
                                .FirstOrDefault(p => p.Periode.Trim() == collectieve.Periode.Trim());

                            if (collectieveFound is not null && collectieveFound.ProcessedDate <= collectieve.ProcessedDate)
                            {
                                await _collection.FindOneAndUpdateAsync(w => w.Id == werkgever.Id,
                                    Builders<MongoWerkgeverModel>.Update.PullFilter(w => w.Collectieve,
                                        f => f.Periode == collectieve.Periode));
                            }

                            if (collectieve.ProcessedDate >= (collectieveFound?.ProcessedDate ?? new DateTime(2000, 1, 1)))
                            {
                                await _collection.FindOneAndUpdateAsync(w => w.Id == werkgever.Id,
                                    Builders<MongoWerkgeverModel>.Update.Push(w => w.Collectieve, collectieve));
                            }
                        }
                    }

                    await _collection.FindOneAndUpdateAsync(
                        Builders<MongoWerkgeverModel>.Filter.Eq(w => w.Id, werkgever.Id),
                        Builders<MongoWerkgeverModel>.Update
                            .Set(w => w.Naam, werkgever.Naam)
                            .Set(w => w.Sector, werkgever.Sector)
                            .Set(w => w.FiscaalNummer, werkgever.FiscaalNummer)
                            .Set(w => w.LoonheffingenExtentie, werkgever.LoonheffingenExtentie)
                            .Set(w => w.OmzetbelastingExtentie, werkgever.OmzetbelastingExtentie)
                            .Set(w => w.DatumActiefVanaf, werkgever.DatumActiefVanaf)
                            .Set(w => w.DatumActiefTot, werkgever.DatumActiefTot)
                            .Set(w => w.DateLastModified, werkgever.DateLastModified)
                            .Set(w => w.Klant, werkgever.Klant)
                            .Set(w => w.Actief, werkgever.Actief));
                }
            }

            return ResultsTo.Something(werkgevers);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<List<MongoWerkgeverModel>>().FromException(e);
        }
    }

    public async Task<IFluentResults<List<MongoWerkgeverModel>>> HandleAsync(GetMongoWerkgever request, CancellationToken cancellationToken = default)
    {
        try
        {
            var filter = Builders<MongoWerkgeverModel>.Filter.Eq(w => w.Actief, true);

            if (!string.IsNullOrWhiteSpace(request.Taxno))
            {
                filter &= Builders<MongoWerkgeverModel>.Filter.Eq(w => w.FiscaalNummer, request.Taxno);
            }

            var result = await _collection.FindAsync(filter).Result.ToListAsync();

            if (result is null || !result.Any())
            {
                return ResultsTo.NotFound<List<MongoWerkgeverModel>>("No werkgevers found");
            }

            foreach (var record in result)
            {
                if (record?.Collectieve is null)
                {
                    continue;
                }

                var orderedCollectieve = record.Collectieve.OrderByDescending(x => x.Periode).ToList();
                record.Collectieve = orderedCollectieve;
            }

            return ResultsTo.Success(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<List<MongoWerkgeverModel>>().FromException(e);
        }
    }

    public async Task<IFluentResults<int>> HandleAsync(UpsertFromMongoWerkgever request, CancellationToken cancellationToken = default)
    {
        var result =
            await _context.Werkgever.FirstOrDefaultAsync(w =>
                w.FiscaalNummer == request.Werkgever.FiscaalNummer && w.Actief);

        if (result is null)
        {
            result = new Database.Models.Werkgever
            {
                WerkgeverMongoId = request.Werkgever.Id.ToString(),
                KlantMongoId = request.Werkgever.Klant?.KlantId ?? string.Empty,
                Naam = request.Werkgever.Naam,
                Sector = request.Werkgever.Sector,
                FiscaalNummer = request.Werkgever.FiscaalNummer,
                LoonheffingenExtentie = request.Werkgever.LoonheffingenExtentie,
                OmzetbelastingExtentie = request.Werkgever.OmzetbelastingExtentie,
                DatumActiefVanaf = request.Werkgever.DatumActiefVanaf,
                DatumActiefTot = request.Werkgever.DatumActiefTot,
                Actief = request.Werkgever.Actief,
            };

            _context.Werkgever.Add(result);
        }
        else
        {
            result.KlantMongoId = request.Werkgever?.Klant?.KlantId ?? string.Empty;
            result.WerkgeverMongoId = request.Werkgever?.Id.ToString();
            result.Naam = request.Werkgever.Naam;
            result.Sector = request.Werkgever.Sector;
            result.FiscaalNummer = request.Werkgever.FiscaalNummer;
            result.LoonheffingenExtentie = request.Werkgever.LoonheffingenExtentie;
            result.OmzetbelastingExtentie = request.Werkgever.OmzetbelastingExtentie;
            result.DatumActiefVanaf = request.Werkgever.DatumActiefVanaf;
            result.DatumActiefTot = request.Werkgever.DatumActiefTot;
            result.Actief = request.Werkgever.Actief;
        }

        await _context.SaveChangesAsync();

        return ResultsTo.Success(result.Id);
    }

    public async Task<IFluentResults<List<Database.Models.Werkgever>>> HandleAsync(GetSqlWerkgevers request, CancellationToken cancellationToken = default)
    {
        return ResultsTo.Something(await _context.Werkgever.Where(w => w.Actief).ToListAsync());
    }

    public async Task<IFluentResults<List<WerkgeverWhkPremies>>> HandleAsync(GetSqlWerkgeversWhkPremies request, CancellationToken cancellationToken = default)
    {
        return ResultsTo.Something(await _context.WerkgeverWhkPremies.Where(w => w.Actief && w.WerkgeverId == request.WerkgeverId).ToListAsync());
    }

    public async Task<IFluentResults<bool>> HandleAsync(UpsertMongoWhk request, CancellationToken cancellationToken = default)
    {
        try
        {
            var taxfilter = Builders<MongoWerkgeverModel>.Filter
                .Eq(w => w.Id, request.WerkgeverId);

            taxfilter &= Builders<MongoWerkgeverModel>.Filter.Eq(w => w.Actief, true);

            var result = await _collection.FindAsync(taxfilter).Result.ToListAsync();

            if (result is null || !result.Any())
            {
                return ResultsTo.NotFound<bool>("No werkgevers found");
            }

            var found = result.FirstOrDefault();
            var whkFound = found?.WhkPremies.FirstOrDefault(p => p.Id == request.Id && p.Actief);

            var whk = new MongoWhkPremie
            {
                Id = whkFound?.Id ?? ObjectId.GenerateNewId(),
                WgaVastWerkgever = request.WgaVastWerkgever,
                WgaVastWerknemer = request.WgaVastWerknemer,
                FlexWerkgever = request.FlexWerkgever,
                FlexWerknemer = request.FlexWerknemer,
                ZwFlex = request.ZwFlex,
                ActiefVanaf = request.ActiefVanaf,
                ActiefTot = request.ActiefTot,
                DateCreated = whkFound?.DateCreated ?? DateTime.Now,
                DateLastModified = DateTime.Now,
                SqlId = whkFound?.SqlId ?? 0,
                Actief = request.Actief,
            };

            if (whkFound is not null)
            {
                await _collection.FindOneAndUpdateAsync(w => w.Id == request.WerkgeverId, Builders<MongoWerkgeverModel>.Update
                    .PullFilter(w => w.WhkPremies, f => f.Id == whk.Id));
            }

            MongoWerkgeverModel updated = null;
            if (request.Actief)
            {
                updated = await _collection.FindOneAndUpdateAsync(w => w.Id == request.WerkgeverId, Builders<MongoWerkgeverModel>.Update.Push(w => w.WhkPremies, whk));
            }

            return ResultsTo.Something(updated != null);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<bool>().FromException(e);
        }
    }

    public async Task<IFluentResults<List<CollectieveAangifteModel>>> HandleAsync(GetCollectieve request, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.TaxNo))
            {
                return ResultsTo.BadRequest<List<CollectieveAangifteModel>>("TaxNo is required").WithMessage("Argument is null or empty");
            }

            var taxfilter = Builders<MongoWerkgeverModel>.Filter
                .Eq(w => w.FiscaalNummer, request.TaxNo);

            taxfilter &= Builders<MongoWerkgeverModel>.Filter.Eq(w => w.Actief, true);

            var data = await _collection.FindAsync(taxfilter).Result.FirstOrDefaultAsync();

            if (data?.Collectieve is null || !data.Collectieve.Any())
            {
                return ResultsTo.NotFound<List<CollectieveAangifteModel>>("No collectieve aangifte found");
            }

            return ResultsTo.Something(data.Collectieve.OrderBy(w => w.Periode).ToList());
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<List<CollectieveAangifteModel>>().FromException(e);
        }
    }

    public async Task<IFluentResults<bool>> HandleAsync(UpdateKlantWerkgever request, CancellationToken cancellationToken = default)
    {
        try
        {
            var taxfilter = Builders<MongoWerkgeverModel>.Filter.Eq(w => w.FiscaalNummer, request.TaxNo);
            taxfilter &= Builders<MongoWerkgeverModel>.Filter.Eq(w => w.Actief, true);

            var result = await _collection.FindOneAndUpdateAsync(
                taxfilter,
                Builders<MongoWerkgeverModel>.Update
                    .Set(w => w.Klant, new WerkgeverKlantModel
                    {
                        KlantId = request.KlantId,
                        KlantName = request.KlantName,
                    }));

            return ResultsTo.Success(true);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<bool>().FromException(e);
        }
    }

    public async Task<IFluentResults> HandleAsync(UpsertFromMongoWerkgeverWhk request, CancellationToken cancellationToken = default)
    {
        if (request.Werkgever.WhkPremies is null || !request.Werkgever.WhkPremies.Any())
        {
            _logger.LogInformation("No WhkPremie to Upsert.");
            return ResultsTo.BadRequest("No WhkPremie to Upsert.").WithMessage("Argument is null or empty");
        }

        var whks = request.Werkgever.WhkPremies;

        foreach (var whk in whks)
        {
            var result =
                await _context.WerkgeverWhkPremies.FirstOrDefaultAsync(w =>
                    w.WerkgeverId == request.WerkgeverId &&
                    (w.WerkgeverWhkMongoId == whk.Id.ToString() || w.Id == whk.SqlId) && w.Actief);

            if (result is null)
            {
                result = new WerkgeverWhkPremies
                {
                    WerkgeverWhkMongoId = whk.Id.ToString(),
                    WgaVastWerkgever = 1,
                    WgaVastWerknemer = 1,
                    FlexWerkgever = 1,
                    FlexWerknemer = 1,
                    ZwFlex = 1,
                    ActiefVanaf = new DateTime(DateTime.Now.Year, 1, 1),
                    ActiefTot = new DateTime(DateTime.Now.Year, 12, DateTime.DaysInMonth(DateTime.Now.Year, 12)),
                    Actief = true,
                };

                result.WerkgeverId = request.WerkgeverId;
                _context.WerkgeverWhkPremies.Add(result);
            }
            else
            {
                result.WerkgeverWhkMongoId = string.IsNullOrWhiteSpace(result.WerkgeverWhkMongoId) ? whk.Id.ToString() : result.WerkgeverWhkMongoId;
                result.WgaVastWerkgever = whk.WgaVastWerkgever;
                result.WgaVastWerknemer = whk.WgaVastWerknemer;
                result.FlexWerkgever = whk.FlexWerkgever;
                result.FlexWerknemer = whk.FlexWerknemer;
                result.ZwFlex = whk.ZwFlex;
                result.Totaal = whk.Totaal;
                result.ActiefVanaf = whk.ActiefVanaf;
                result.ActiefTot = whk.ActiefTot;
            }

            await _context.SaveChangesAsync();
        }

        return ResultsTo.Success();
    }
}