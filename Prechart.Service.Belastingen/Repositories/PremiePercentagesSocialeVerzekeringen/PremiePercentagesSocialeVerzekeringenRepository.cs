using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Prechart.Service.Belastingen.Database.Context;
using Prechart.Service.Belastingen.Models.PremiePercentagesSocialeVerzekeringen;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Outcomes;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Prechart.Service.Belastingen.Repositories.PremiePercentagesSocialeVerzekeringen;

public partial class PremiePercentagesSocialeVerzekeringenRepository : IPremiePercentagesSocialeVerzekeringenRepository
{
    private readonly IBelastingenDbContext _context;
    private readonly ILogger<PremiePercentagesSocialeVerzekeringenRepository> _logger;

    public PremiePercentagesSocialeVerzekeringenRepository(ILogger<PremiePercentagesSocialeVerzekeringenRepository> logger, IBelastingenDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IFluentResults<PremieBedragModel>> HandleAsync(GetPremiePercentage request, CancellationToken cancellationToken)
    {
        var premieRecord = await _context.PremiePercentagesSocialeVerzekeringen
            .Where(p => request.SocialeVerzekeringenDatum.Date >= p.SocialeVerzekeringenRecordActiefVanaf.Date && request.SocialeVerzekeringenDatum.Date <= p.SocialeVerzekeringenRecordActiefTot.Date)
            .FirstOrDefaultAsync(p => p.SocialeVerzekeringenRecordActief);


        if (premieRecord is null)
        {
            return ResultsTo.NotFound<PremieBedragModel>();
        }

        var result = PopuplatePremieBedragModel(premieRecord, request);
        return ResultsTo.Success<PremieBedragModel>(result);
    }

    private static PremieBedragModel PopuplatePremieBedragModel(Database.Models.PremiePercentagesSocialeVerzekeringen record, GetPremiePercentage request)
    {
        return new PremieBedragModel
        {
            PremieBedragAlgemeneOuderdomsWet = Math.Round(request.LoonSocialVerzekeringen * record.PremiePercentageAlgemeneOuderdomsWet / 100M, 2, MidpointRounding.AwayFromZero),
            PremieBedragNabestaanden = Math.Round(request.LoonSocialVerzekeringen * record.PremiePercentageNabestaanden / 100M, 2, MidpointRounding.AwayFromZero),
            PremieBedragWetLangdurigeZorg = Math.Round(request.LoonSocialVerzekeringen * record.PremiePercentageWetLangdurigeZorg / 100M, 2, MidpointRounding.AwayFromZero),
            PremieBedragSocialeVerzekeringenPremieloon = Math.Round(request.LoonSocialVerzekeringen, 2, MidpointRounding.AwayFromZero),
            PremieBedragAlgemeenWerkloosheidsFondsLaag = Math.Round(request.LoonSocialVerzekeringen * record.PremiePercentageAlgemeenWerkloosheidsFondsLaag / 100M, 2, MidpointRounding.AwayFromZero),
            PremieBedragAlgemeenWerkloosheidsFondsHoog = Math.Round(request.LoonSocialVerzekeringen * record.PremiePercentageAlgemeenWerkloosheidsFondsHoog / 100M, 2, MidpointRounding.AwayFromZero),
            PremieBedragUitvoeringsFondsvoordeOverheid = Math.Round(request.LoonSocialVerzekeringen * record.PremiePercentageUitvoeringsdfondsvoordeOverheid / 100M, 2, MidpointRounding.AwayFromZero),
            PremieBedragWetArbeidsOngeschikheidLaag = Math.Round(request.LoonSocialVerzekeringen * record.PremiePercentageWetArbeidsongeschikheidLaag / 100M, 2, MidpointRounding.AwayFromZero),
            PremieBedragWetArbeidsOngeschikheidHoog = Math.Round(request.LoonSocialVerzekeringen * record.PremiePercentageWetArbeidsongeschikheidHoog / 100M, 2, MidpointRounding.AwayFromZero),
            PremieBedragWetKinderopvang = Math.Round(request.LoonSocialVerzekeringen * record.PremiePercentageWetKinderopvang / 100M, 2, MidpointRounding.AwayFromZero),
            PremieBedragZiektekostenVerzekeringsWetWerkgeversbijdrage = Math.Round(request.LoonZiektekostenVerzekeringsWet * record.PremiePercentageZiektekostenverzekeringWerkgeversbijdrage / 100M, 2, MidpointRounding.AwayFromZero),
            PremieBedragZiektekostenVerzekeringsWetWerknemersbijdrage = Math.Round(request.LoonZiektekostenVerzekeringsWet * record.PremiePercentageZiektekostenverzekeringWerknemersbijdrage / 100M, 2, MidpointRounding.AwayFromZero),
            PremieBedragZiektekostenVerzekeringsWetLoon = Math.Round(request.LoonZiektekostenVerzekeringsWet, 2, MidpointRounding.AwayFromZero),
        };
    }
}
