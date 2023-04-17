using MassTransit;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Globals.Interfaces.Werkgever;
using Prechart.Service.Globals.Models;
using Prechart.Service.Werkgever.Service;

namespace Prechart.Service.Werkgever.Consumers;

public class GetWerkgeversConsumer : IConsumer<IMongoGetWerkgever>
{
    private readonly ILogger<GetWerkgeversConsumer> _logger;
    private readonly IWerkgeverService _service;

    public GetWerkgeversConsumer(ILogger<GetWerkgeversConsumer> logger, IWerkgeverService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task Consume(ConsumeContext<IMongoGetWerkgever> context)
    {
        _logger.LogInformation($"{nameof(IMongoGetWerkgever)} event with payload {context.Message}");

        var message = context.Message;

        var result = await _service.HandleAsync(new WerkgeverService.GetMongoWerkgever
        {
            Taxno = message.Taxno,
        }, CancellationToken.None);

        if (result is null || result.IsNotFoundOrBadRequest())
        {
            await context.RespondAsync(new NotFound());
        }

        await context.RespondAsync<IMongoWerkgevers>(new {Werkgevers = result?.Value});
    }
}