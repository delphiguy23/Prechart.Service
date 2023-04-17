using MassTransit;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Globals.Interfaces.Klant;
using Prechart.Service.Globals.Models;
using Prechart.Service.Klant.Service;

namespace Prechart.Service.Klant.Consumers;

public class GetAllKlantsConsumer : IConsumer<IGetAllKlants>
{
    private readonly ILogger<GetAllKlantsConsumer> _logger;
    private readonly IKlantService _service;

    public GetAllKlantsConsumer(ILogger<GetAllKlantsConsumer> logger, IKlantService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task Consume(ConsumeContext<IGetAllKlants> context)
    {
        _logger.LogInformation($"{nameof(IGetAllKlants)} event with payload {context.Message}");

        try
        {
            var result = await _service.HandleAsync(new KlantService.GetKlants(), CancellationToken.None);

            if (result?.Value is null || result.IsNotFoundOrBadRequest())
            {
                await context.RespondAsync(new NotFound());
            }

            await context.RespondAsync<IKlants>(new {Klants = result?.Value});
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(Consume));
        }
    }
}
