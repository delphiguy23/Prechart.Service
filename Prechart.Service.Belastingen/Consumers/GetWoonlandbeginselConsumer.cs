using MassTransit;
using Microsoft.Extensions.Logging;
using Prechart.Service.Belastingen.Database.Models;
using Prechart.Service.Belastingen.Services.BelastingTabellenWitGroen;
using Prechart.Service.Globals.Interfaces.Belastingen;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Prechart.Service.Belastingen.Consumers;

public class GetWoonlandbeginselConsumer : IConsumer<IGetWoonlandbeginselEvent>
{
    private readonly ILogger<GetWoonlandbeginselConsumer> _logger;
    private readonly IBelastingTabellenWitGroenService _service;

    public GetWoonlandbeginselConsumer(ILogger<GetWoonlandbeginselConsumer> logger, IBelastingTabellenWitGroenService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task Consume(ConsumeContext<IGetWoonlandbeginselEvent> context)
    {
        _logger.LogInformation($"{nameof(IGetWoonlandbeginselEvent)} event with payload {context.Message}");

        try
        {
            var result = await _service.HandleAsync(new BelastingTabellenWitGroenService.GetAlleWoonlandbeginsel(), CancellationToken.None);

            if (result is null)
            {
                await context.RespondAsync<IWoonlandbeginsels>(new Woonlandbeginsel());
            }

            await context.RespondAsync<IWoonlandbeginsels>(new { Woonlandbeginsels = result.Value });
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred on fetching Woonlandbeginsel on consumer", ex);
            await context.RespondAsync<IWoonlandbeginsels>(new Woonlandbeginsel());
        }
    }
}
