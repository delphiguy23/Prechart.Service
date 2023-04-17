using Microsoft.Extensions.Logging;
using Prechart.Service.Belastingen.Models.PremiePercentagesSocialeVerzekeringen;
using Prechart.Service.Belastingen.Repositories.PremiePercentagesSocialeVerzekeringen;
using Prechart.Service.Core.FluentResults;
using System.Threading;
using System.Threading.Tasks;

namespace Prechart.Service.Belastingen.Services.PremiePercentagesSocialeVerzekeringen;

public partial class PremiePercentagesSocialeVerzekeringenService : IPremiePercentagesSocialeVerzekeringenService
{
    private readonly ILogger<PremiePercentagesSocialeVerzekeringenService> _logger;
    private readonly IPremiePercentagesSocialeVerzekeringenRepository _repository;

    public PremiePercentagesSocialeVerzekeringenService(IPremiePercentagesSocialeVerzekeringenRepository repository, ILogger<PremiePercentagesSocialeVerzekeringenService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IFluentResults<PremieBedragModel>> HandleAsync(GetPremiePercentage request, CancellationToken cancellationToken)
    {
        return await _repository.HandleAsync(new PremiePercentagesSocialeVerzekeringenRepository.GetPremiePercentage
        {
            LoonSocialVerzekeringen = request.LoonSocialVerzekeringen,
            LoonZiektekostenVerzekeringsWet = request.LoonZiektekostenVerzekeringsWet,
            SocialeVerzekeringenDatum = request.SocialeVerzekeringenDatum,
        }, CancellationToken.None);
    }
}
