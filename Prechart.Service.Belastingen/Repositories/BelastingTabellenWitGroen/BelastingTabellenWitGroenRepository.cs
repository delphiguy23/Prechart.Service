using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Prechart.Service.Belastingen.Database.Context;
using Prechart.Service.Belastingen.Database.Models;
using Prechart.Service.Belastingen.Models;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Core.Service;
using Prechart.Service.Globals.Models.Belastingen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Prechart.Service.Belastingen.Repositories.BelastingTabellenWitGroen;

public partial class BelastingTabellenWitGroenRepository : IBelastingTabellenWitGroenRepository
{
    private readonly IBelastingenDbContext _context;
    private readonly ILogger<BelastingTabellenWitGroenRepository> _logger;
    private readonly IMapper _mapper;
    private readonly Action<int> _timeout;

    public BelastingTabellenWitGroenRepository(ILogger<BelastingTabellenWitGroenRepository> logger,
        IBelastingenDbContext context, IMapper mapper)
        : this(logger, context, mapper, to => { context.Database.SetCommandTimeout(to); })
    {
    }

    public BelastingTabellenWitGroenRepository(ILogger<BelastingTabellenWitGroenRepository> logger,
        IBelastingenDbContext context, IMapper mapper, Action<int> timeout)
    {
        _logger = logger;
        _context = context;
        _timeout = timeout;
        _mapper = mapper;
    }

    public async Task<IFluentResults<int>> HandleAsync(GetWoonlandbeginselCode request, CancellationToken cancellationToken)
    {
        var result = await _context.Woonlandbeginsel.FirstOrDefaultAsync(c =>
            c.WoonlandbeginselCode == request.WoonlandbeginselCode && c.Active);

        return ResultsTo.Success<int>(result.Id) ?? ResultsTo.NotFound<int>();
    }

    public async Task<IFluentResults<string>> HandleAsync(GetWoonlandbeginselId request, CancellationToken cancellationToken)
    {
        var result =
            await _context.Woonlandbeginsel.FirstOrDefaultAsync(c => c.Id == request.WoonlandbeginselId && c.Active);

        return result is not null ? ResultsTo.Success<string>(result?.WoonlandbeginselBenaming) : ResultsTo.NotFound<string>();
    }

    public async Task<IFluentResults<List<Woonlandbeginsel>>> HandleAsync(GetWoonlandbeginsel request, CancellationToken cancellationToken)
    {
        var result = await _context.Woonlandbeginsel
            .Where(c => c.Active)
            .ToListAsync();

        return ResultsTo.Success<List<Woonlandbeginsel>>(result);
    }

    public async Task<IFluentResults<BerekenInhoudingModel>> HandleAsync(GetInhoudingGreen request, CancellationToken cancellationToken)
    {
        try
        {
            _timeout(3200);

            if (request.InkomenGroen == 0)
            {
                return ResultsTo.NotFound<BerekenInhoudingModel>();
            }


            var basisDagen = request.BasisDagen;
            var inkomenGroen = request.InkomenGroen;

            if (request.Loontijdvak != TaxPeriodEnum.Day)
            {
                basisDagen = 1;
            }
            else
            {
                inkomenGroen /= basisDagen;
            }

            var isAow = await HandleAsync(new IsAow { Geboortedatum = request.Geboortedatum }, CancellationToken.None) is IsSome<DateTime> aow &&
                        new DateTime(aow.Value.Year, aow.Value.Month, DateTime.DaysInMonth(aow.Value.Year, aow.Value.Month)) <=
                        DateTime.Now.Date;

            var laandNaam = (await HandleAsync(new GetWoonlandbeginselId { WoonlandbeginselId = request.WoondlandBeginselId }, CancellationToken.None))?.Value ?? string.Empty;

            var result = await _context.Green
                .Where(g => g.Tabelloon <= inkomenGroen)
                .Where(g => g.CountryId == request.WoondlandBeginselId)
                .Where(g => g.Year == request.Jaar)
                .Where(g => g.TypeId == (int)request.Loontijdvak)
                .OrderByDescending(g => g.Tabelloon)
                .Select(g => new BerekenInhoudingModel
                {
                    InhoudingWit = 0M,
                    InhoudingGroen = request.AhkInd
                        ? (GetValues(isAow, g.MetLoonheffingskorting, g.LaterMetLoonheffingskorting) + GetValues(isAow, g.VerrekendeArbeidskorting, g.LaterVerrekendeArbeidskorting)) * basisDagen
                        : GetValues(isAow, g.ZonderLoonheffingskorting, g.LaterZonderLoonheffingskorting) * basisDagen,
                    AlgemeneHeffingsKorting = request.AhkInd
                        ? (
                            GetValues(isAow, g.ZonderLoonheffingskorting, g.LaterZonderLoonheffingskorting) -
                            (
                                GetValues(isAow, g.MetLoonheffingskorting, g.LaterMetLoonheffingskorting) +
                                GetValues(isAow, g.VerrekendeArbeidskorting, g.LaterVerrekendeArbeidskorting)
                            )
                        ) * basisDagen
                        : 0M,
                    ArbeidsKorting = request.AhkInd
                        ? GetValues(isAow, g.VerrekendeArbeidskorting, g.LaterVerrekendeArbeidskorting) * basisDagen
                        : 0M,
                    AlgemeneHeffingsKortingIndicator = request.AhkInd,
                    InhoudingType = TaxRecordType.Green,
                    WoonlandbeginselId = request.WoondlandBeginselId,
                    WoonlandbeginselNaam = laandNaam,
                })
                .FirstOrDefaultAsync();

            return ResultsTo.Success<BerekenInhoudingModel>(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return ResultsTo.Failure<BerekenInhoudingModel>(ex.Message);
        }
    }

    public async Task<IFluentResults<BerekenInhoudingModel>> HandleAsync(GetInhoudingWhite request, CancellationToken cancellationToken)
    {
        try
        {
            _timeout(3200);

            if (request.InkomenWit == 0)
            {
                return ResultsTo.NotFound<BerekenInhoudingModel>();
            }

            var basisDagen = request.BasisDagen;
            var inkomenWit = request.InkomenWit;

            if (request.Loontijdvak != TaxPeriodEnum.Day)
            {
                basisDagen = 1;
            }
            else
            {
                inkomenWit /= basisDagen;
            }

            var isAow = await HandleAsync(new IsAow { Geboortedatum = request.Geboortedatum }, CancellationToken.None) is IsSome<DateTime> aow &&
                        new DateTime(aow.Value.Year, aow.Value.Month, DateTime.DaysInMonth(aow.Value.Year, aow.Value.Month)) <=
                        DateTime.Now.Date;

            var laandNaam = (await HandleAsync(new GetWoonlandbeginselId { WoonlandbeginselId = request.WoondlandBeginselId }, CancellationToken.None)).Value ?? string.Empty;

            var result = await _context.White
                .Where(w => w.Tabelloon <= inkomenWit)
                .Where(w => w.CountryId == request.WoondlandBeginselId)
                .Where(w => w.Year == request.Jaar)
                .Where(w => w.TypeId == (int)request.Loontijdvak)
                .OrderByDescending(w => w.Tabelloon)
                .Select(w => new BerekenInhoudingModel
                {
                    InhoudingWit = request.AhkInd
                        ? (GetValues(isAow, w.MetLoonheffingskorting, w.LaterMetLoonheffingskorting) + GetValues(isAow, w.VerrekendeArbeidskorting, w.LaterVerrekendeArbeidskorting)) * basisDagen
                        : GetValues(isAow, w.ZonderLoonheffingskorting, w.LaterZonderLoonheffingskorting) * basisDagen,
                    InhoudingGroen = 0M,
                    AlgemeneHeffingsKorting = request.AhkInd
                        ? (
                            GetValues(isAow, w.ZonderLoonheffingskorting, w.LaterZonderLoonheffingskorting) -
                            (
                                GetValues(isAow, w.MetLoonheffingskorting, w.LaterMetLoonheffingskorting) +
                                GetValues(isAow, w.VerrekendeArbeidskorting, w.LaterVerrekendeArbeidskorting)
                            )
                        ) * basisDagen
                        : 0M,
                    ArbeidsKorting = request.AhkInd
                        ? GetValues(isAow, w.VerrekendeArbeidskorting, w.LaterVerrekendeArbeidskorting) * basisDagen
                        : 0M,
                    AlgemeneHeffingsKortingIndicator = request.AhkInd,
                    InhoudingType = TaxRecordType.White,
                    WoonlandbeginselId = request.WoondlandBeginselId,
                    WoonlandbeginselNaam = laandNaam,
                })
                .FirstOrDefaultAsync();

            return ResultsTo.Success<BerekenInhoudingModel>(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return ResultsTo.Failure<BerekenInhoudingModel>(ex.Message);
        }
    }

    public async Task<IFluentResults<BerekenInhoudingModel>> HandleAsync(GetInhoudingBoth request, CancellationToken cancellationToken)
    {
        try
        {
            _timeout(3200);

            if (request.InkomenWit == 0M && request.InkomenGroen == 0M)
            {
                return ResultsTo.NotFound<BerekenInhoudingModel>();
            }

            var basisDagen = request.BasisDagen;
            var inkomenWit = request.InkomenWit;
            var inkomenGroen = request.InkomenGroen;

            if (request.Loontijdvak != TaxPeriodEnum.Day)
            {
                basisDagen = 1;
            }
            else
            {
                inkomenGroen /= basisDagen;
                inkomenWit /= basisDagen;
            }

            var isAow = await HandleAsync(new IsAow { Geboortedatum = request.Geboortedatum }, CancellationToken.None) is IsSome<DateTime> aow &&
                        new DateTime(aow.Value.Year, aow.Value.Month, DateTime.DaysInMonth(aow.Value.Year, aow.Value.Month)) <=
                        DateTime.Now.Date;

            var laandNaam = (await HandleAsync(new GetWoonlandbeginselId { WoonlandbeginselId = request.WoondlandBeginselId }, CancellationToken.None)).Value ?? string.Empty;

            var result = await _context.White
                .Where(b => b.Tabelloon <= inkomenWit + inkomenGroen)
                .Where(b => b.CountryId == request.WoondlandBeginselId)
                .Where(b => b.Year == request.Jaar)
                .Where(b => b.TypeId == (int)request.Loontijdvak)
                .OrderByDescending(b => b.Tabelloon)
                .Select(b => new BerekenInhoudingModel
                {
                    InhoudingWit = request.AhkInd
                        ? (
                            GetValues(isAow, b.MetLoonheffingskorting, b.LaterMetLoonheffingskorting) +
                            GetValues(isAow, b.VerrekendeArbeidskorting, b.LaterVerrekendeArbeidskorting)
                        ) * basisDagen
                        : GetValues(isAow, b.ZonderLoonheffingskorting, b.LaterZonderLoonheffingskorting) * basisDagen,
                    InhoudingGroen =
                        request.AhkInd ? 0M : 0M, //What should the value of InkomenGroen if AhkInd = 0 ????????
                    AlgemeneHeffingsKorting = request.AhkInd
                        ? (
                            GetValues(isAow, b.ZonderLoonheffingskorting, b.LaterZonderLoonheffingskorting) -
                            (
                                GetValues(isAow, b.MetLoonheffingskorting, b.LaterMetLoonheffingskorting) +
                                GetValues(isAow, b.VerrekendeArbeidskorting, b.LaterVerrekendeArbeidskorting)
                            )
                        ) * basisDagen
                        : 0M,
                    ArbeidsKorting = request.AhkInd
                        ? GetValues(isAow, b.VerrekendeArbeidskorting, b.LaterVerrekendeArbeidskorting) * basisDagen
                        : 0M,
                    AlgemeneHeffingsKortingIndicator = request.AhkInd,
                    InhoudingType = TaxRecordType.Both,
                    WoonlandbeginselId = request.WoondlandBeginselId,
                    WoonlandbeginselNaam = laandNaam,
                })
                .FirstOrDefaultAsync();

            return ResultsTo.Success<BerekenInhoudingModel>(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return ResultsTo.Failure<BerekenInhoudingModel>(ex.Message);
        }
    }

    public async Task<IFluentResults<int>> HandleAsync(UpsertToTable request, CancellationToken cancellationToken)
    {
        try
        {
            _timeout(1000);

            var getRecords = request.TaxTable.Select(t => new { t.Year, t.TypeId, t.CountryId }).ToList();
            var taxType = request.TaxType.ToUpper();

            if (taxType == TaxTypeEnum.Wit.ToString().ToUpper())
            {
                var existingWhites = await _context.White.Where(w =>
                    getRecords.Select(r => r.Year).Contains(w.Year) &&
                    getRecords.Select(r => r.TypeId).Contains(w.TypeId) &&
                    getRecords.Select(r => r.CountryId).Contains(w.CountryId)).ToListAsync();

                _context.White.RemoveRange(existingWhites);

                await _context.White.AddRangeAsync(_mapper.Map<List<White>>(request.TaxTable));
            }

            if (taxType == TaxTypeEnum.Groen.ToString().ToUpper())
            {
                var existingGreens = await _context.Green.Where(w =>
                    getRecords.Select(r => r.Year).Contains(w.Year) &&
                    getRecords.Select(r => r.TypeId).Contains(w.TypeId) &&
                    getRecords.Select(r => r.CountryId).Contains(w.CountryId)).ToListAsync();

                _context.Green.RemoveRange(existingGreens);

                await _context.Green.AddRangeAsync(_mapper.Map<List<Green>>(request.TaxTable));
            }

            return ResultsTo.Success<int>(await _context.SaveChangesAsync());
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);
            return ResultsTo.Failure<int>(e.Message);
        }
    }

    public async Task<IFluentResults<List<int>>> HandleAsync(GetTaxYear request, CancellationToken cancellationToken)
    {
        var whiteYear = await _context.White
            .Where(x => x.Active == true)
            .Select(x => x.Year)
            .Distinct().ToListAsync();

        var greenYear = await _context.Green
            .Where(x => x.Active == true)
            .Select(x => x.Year)
            .Distinct().ToListAsync();

        var bothYear = whiteYear.Union(greenYear).ToList();

        if (!bothYear.Any())
        {
            return ResultsTo.NotFound<List<int>>();
        }

        return ResultsTo.Success<List<int>>(bothYear.OrderByDescending(x => x).ToList());
    }

    public async Task<IFluentResults<DateTime>> HandleAsync(IsAow request, CancellationToken cancellationToken)
    {
        var Aows = await _context.AOW.ToListAsync();

        var result = Aows.Where(a => a.GeborenNa < request.Geboortedatum && request.Geboortedatum < a.GeborenVoor)
            .OrderBy(a => a.GerechtigdIn).FirstOrDefault();

        if (result is null)
        {
            return ResultsTo.NotFound<DateTime>();
        }

        if (result.GerechtigdIn is null)
        {
            result.GerechtigdIn = Aows.Max(s => s.GerechtigdIn);
        }

        if (result.ExtraMaandenNa65 is null)
        {
            result.ExtraMaandenNa65 = Aows.Max(s => s.ExtraMaandenNa65);
        }

        var pensionDate = request.Geboortedatum.AddYears(65);

        return ResultsTo.Success<DateTime>(pensionDate.AddMonths(result.ExtraMaandenNa65 ?? 0));
    }

    private static decimal GetValues(bool? isAow, decimal normalAmount, decimal aowAmount)
    {
        return isAow ?? false ? aowAmount : normalAmount;
    }
}
