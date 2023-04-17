using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Prechart.Service.Globals.Interfaces.Batch;
using Prechart.Service.Loonheffings.Service;

namespace Prechart.Service.Loonheffings.Consumer;

public class PendingXmlBatchProcessConsumer : IConsumer<IBatchTriggerNow>
{
    private readonly ILogger<PendingXmlBatchProcessConsumer> _logger;
    private readonly ILoonheffingsService _service;

    public PendingXmlBatchProcessConsumer(ILogger<PendingXmlBatchProcessConsumer> logger, ILoonheffingsService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task Consume(ConsumeContext<IBatchTriggerNow> context)
    {
        _logger.LogInformation($"{nameof(IBatchTriggerNow)} event with payload {context.Message}");

        if (context.Message is IBatchTriggerNow && isValidBatchNameToProcess(context.Message))
        {
            await _service.HandleAsync(new LoonheffingsService.ProcessUploadedXmls(), CancellationToken.None);
        }

        _logger.LogInformation($"{nameof(IBatchTriggerNow)} event has been consumed.");
    }

    private bool isValidBatchNameToProcess(IBatchTriggerNow request)
    {
        return request.BatchName.ToUpperInvariant() == "XmlUpload".ToUpperInvariant();
    }
}
