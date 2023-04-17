using MassTransit;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Globals.Interfaces.Klant;
using Prechart.Service.Globals.Models;
using Prechart.Service.Klant.Service;

namespace Prechart.Service.Klant.Consumers;

public class GetKlantConsumer : IConsumer<IGetKlant>
{
    private readonly ILogger<GetKlantConsumer> _logger;
    private readonly IKlantService _service;

    public GetKlantConsumer(ILogger<GetKlantConsumer> logger, IKlantService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task Consume(ConsumeContext<IGetKlant> context)
    {
        _logger.LogInformation($"{nameof(IGetKlant)} event with payload {context.Message}");

        try
        {
            var result = await _service.HandleAsync(new KlantService.GetKlant
            {
                KlantName = context.Message.KlantName,
                KlantId = context.Message.KlantId,
                TaxNo = context.Message.TaxNo,
            }, CancellationToken.None);

            if (result?.Value is null || result.IsNotFoundOrBadRequest())
            {
                await context.RespondAsync(new NotFound());
            }

            if (result?.Value != null)
            {
                var klant = new
                {
                    Id = result?.Value.Id.ToString(),
                    result?.Value.KlantNaam,
                    result?.Value.Werkgevers,
                    result?.Value.ContactPersons,
                    result?.Value.Active,
                    result?.Value.DateCreated,
                    result?.Value.DateLastModified,
                };

                await context.RespondAsync<IKlantResponse>(klant);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(Consume));
        }
    }
}
