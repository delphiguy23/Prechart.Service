using MassTransit;
using Microsoft.Extensions.Logging;
using Prechart.Service.AuditLog.Interfaces.PremieBedrag;
using Prechart.Service.Belastingen.Services.PremiePercentagesSocialeVerzekeringen;
using Prechart.Service.Globals.Interfaces.PremieBedrag;
using Prechart.Service.Globals.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Prechart.Service.Belastingen.Consumers;

public class PremieBedragConsumer : IConsumer<IGetPremieBedrag>
{
    private readonly ILogger<PremieBedragConsumer> _logger;
    private readonly IPremiePercentagesSocialeVerzekeringenService _service;

    public PremieBedragConsumer(ILogger<PremieBedragConsumer> logger,
        IPremiePercentagesSocialeVerzekeringenService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task Consume(ConsumeContext<IGetPremieBedrag> context)
    {
        _logger.LogInformation($"{nameof(IGetPremieBedrag)} event with payload {context.Message}");

        var result = await _service.HandleAsync(new PremiePercentagesSocialeVerzekeringenService.GetPremiePercentage
        {
            LoonSocialVerzekeringen = context.Message.LoonSocialVerzekeringen,
            LoonZiektekostenVerzekeringsWet = context.Message.LoonZiektekostenVerzekeringsWet,
            SocialeVerzekeringenDatum = context.Message.SocialeVerzekeringenDatum,
        }, CancellationToken.None);

        if (result is null)
        {
            await context.RespondAsync(new NotFound());
        }

        await context.RespondAsync<IPremieBedrag>(result?.Value);
    }
}
