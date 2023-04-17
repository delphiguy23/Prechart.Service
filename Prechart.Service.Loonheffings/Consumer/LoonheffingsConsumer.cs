using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Prechart.Service.Core.FluentResults.Extension;
using Prechart.Service.Globals.Interfaces.Documents;
using Prechart.Service.Globals.Models;
using Prechart.Service.Loonheffings.Service;

namespace Prechart.Service.Loonheffings.Consumer;

public class LoonheffingsConsumer : IConsumer<ILoonaangifteUploadEvent>
{
    private readonly ILogger<LoonheffingsConsumer> _logger;
    private readonly ILoonheffingsService _service;

    public LoonheffingsConsumer(ILogger<LoonheffingsConsumer> logger, ILoonheffingsService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task Consume(ConsumeContext<ILoonaangifteUploadEvent> context)
    {
        _logger.LogInformation($"{nameof(ILoonaangifteUploadEvent)} event with payload {context.Message}");

        try
        {
            var result = await _service.HandleAsync(new LoonheffingsService.ValidateXml
            {
                Files = context.Message.Files,
                XsdYear = context.Message.XsdYear,
            }, CancellationToken.None);

            if (result?.Value is null || result.IsNotFoundOrBadRequest())
            {
                await context.RespondAsync(new NotFound());
            }

            await context.RespondAsync(result?.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(Consume));
        }
    }
}
