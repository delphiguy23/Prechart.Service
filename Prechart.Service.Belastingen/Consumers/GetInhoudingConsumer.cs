using MassTransit;
using Microsoft.Extensions.Logging;
using Prechart.Service.Belastingen.Services.BelastingTabellenWitGroen;
using Prechart.Service.Globals.Interfaces.Belastingen;
using Prechart.Service.Globals.Models;
using Prechart.Service.Globals.Models.Belastingen;
using System.Threading;
using System.Threading.Tasks;

namespace Prechart.Service.Belastingen.Consumers;

public class GetInhoudingConsumer : IConsumer<IGetInhouding>
{
    private readonly ILogger<GetInhoudingConsumer> _logger;
    private readonly IBelastingTabellenWitGroenService _service;

    public GetInhoudingConsumer(ILogger<GetInhoudingConsumer> logger, IBelastingTabellenWitGroenService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task Consume(ConsumeContext<IGetInhouding> context)
    {
        _logger.LogInformation($"{nameof(IGetInhouding)} event with payload {context.Message}");

        var result = await _service.HandleAsync(new BelastingTabellenWitGroenService.GetInhouding
        {
            WoondlandBeginselId = context.Message.WoondlandBeginselId,
            ProcesDatum = context.Message.ProcesDatum,
            Loontijdvak = context.Message.Loontijdvak,
            InkomenWit = context.Message.InkomenWit,
            InkomenGroen = context.Message.InkomenGroen,
            AlgemeneHeffingsKortingIndicator = context.Message.AlgemeneHeffingsKortingIndicator,
            BasisDagen = context.Message.BasisDagen,
            Geboortedatum = context.Message.Geboortedatum,
        }, CancellationToken.None);

        if (result?.Value is null)
        {
            await context.RespondAsync(new NotFound());
        }

        await context.RespondAsync<IBerekenenInhouding>(result?.Value);
    }
}
