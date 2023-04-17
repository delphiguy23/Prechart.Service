using AutoMapper;
using MassTransit;
using Prechart.Service.Globals.Interfaces.Werkgever;
using Prechart.Service.Globals.Models;
using Prechart.Service.Globals.Models.Werkgever;
using Prechart.Service.Werkgever.Service;

namespace Prechart.Service.Werkgever.Consumers;

public class UpsertWerkgeversConsumer : IConsumer<IMongoWerkgevers>
{
    private readonly ILogger<UpsertWerkgeversConsumer> _logger;
    private readonly IMapper _mapper;
    private readonly IWerkgeverService _service;

    public UpsertWerkgeversConsumer(ILogger<UpsertWerkgeversConsumer> logger, IWerkgeverService service, IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<IMongoWerkgevers> context)
    {
        _logger.LogInformation($"{nameof(IMongoWerkgevers)} event with payload {context.Message}");

        if (context?.Message?.Werkgevers is null)
        {
            await context.RespondAsync(new NotFound());
        }

        var message = _mapper.Map<List<MongoWerkgeverModel>>(context.Message.Werkgevers);

        var result = await _service.HandleAsync(new WerkgeverService.UpsertMongoWerkgevers
        {
            Werkgevers = message,
        }, CancellationToken.None);
    }
}
