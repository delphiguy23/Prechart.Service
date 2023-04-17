using MassTransit;
using Prechart.Service.Globals.Interfaces.Klant;
using Prechart.Service.Klant.Service;

namespace Prechart.Service.Klant.Consumers;

public class UpsertKlantsConsumer : IConsumer<IXmlToKlants>
{
    private readonly ILogger<UpsertKlantsConsumer> _logger;
    private readonly IKlantService _service;

    public UpsertKlantsConsumer(ILogger<UpsertKlantsConsumer> logger, IKlantService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task Consume(ConsumeContext<IXmlToKlants> context)
    {
        _logger.LogInformation($"{nameof(IXmlToKlants)} event with payload {context.Message}");

        try
        {
            await _service.HandleAsync(new KlantService.UpsertKlants
            {
                Klants = context.Message.Klants,
            }, CancellationToken.None);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(Consume));
        }
    }
}
