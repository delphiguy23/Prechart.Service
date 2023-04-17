using MassTransit;
using Prechart.Service.Globals.Interfaces.Werkgever;
using Prechart.Service.Werkgever.Service;

namespace Prechart.Service.Werkgever.Consumers;

public class UpdateKlantWerkgeverConsumer : IConsumer<IUpsertWerkgever>
{
    private readonly ILogger<UpdateKlantWerkgeverConsumer> _logger;
    private readonly IWerkgeverService _service;

    public UpdateKlantWerkgeverConsumer(ILogger<UpdateKlantWerkgeverConsumer> logger, IWerkgeverService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task Consume(ConsumeContext<IUpsertWerkgever> context)
    {
        if (context.Message is not null)
        {
            _logger.LogInformation($"{nameof(IUpsertWerkgever)} event with payload {context.Message}");

            await _service.HandleAsync(new WerkgeverService.UpdateKlantWerkgever
            {
                KlantId = context.Message.KlantId,
                KlantName = context.Message.KlantName,
                TaxNo = context.Message.TaxNo,
            }, CancellationToken.None);
        }
    }
}
