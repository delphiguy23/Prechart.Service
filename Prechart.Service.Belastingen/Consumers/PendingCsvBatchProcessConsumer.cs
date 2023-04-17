using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Prechart.Service.Belastingen.Services.BelastingTabellenWitGroen;
using Prechart.Service.Globals.Interfaces.Batch;

namespace Prechart.Service.Belastingen.Consumers;

public class PendingCsvBatchProcessConsumer : IConsumer<IPendingBatchRecords>
{
    private readonly ILogger<PendingCsvBatchProcessConsumer> _logger;
    private readonly IBelastingTabellenWitGroenService _service;

    public PendingCsvBatchProcessConsumer(ILogger<PendingCsvBatchProcessConsumer> logger, IBelastingTabellenWitGroenService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task Consume(ConsumeContext<IPendingBatchRecords> context)
    {
        _logger.LogInformation($"{nameof(IPendingBatchRecords)} event with payload {context.Message}");

        if (context.Message is IPendingBatchRecords && context.Message?.BatchProcess is not null && isValidBatchNameToProcess(context.Message))
        {
            await _service.HandleAsync(new BelastingTabellenWitGroenService.ProcessPendingCsv
            {
                BatchRecords = context.Message.BatchProcess,
            }, CancellationToken.None);
        }

        _logger.LogInformation($"{nameof(IPendingBatchRecords)} event has been consumed.");
    }

    private bool isValidBatchNameToProcess(IPendingBatchRecords request)
    {
        return string.Equals(request?.BatchProcess?.BatchName ?? string.Empty, "CsvUpload", StringComparison.InvariantCultureIgnoreCase);
    }
}
