using System;
using System.Threading;
using System.Threading.Tasks;
using Hangfire.Server;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Prechart.Service.Batch.Services;
using Prechart.Service.Core.Configuration;
using Prechart.Service.Core.Scheduling;

namespace Prechart.Service.Batch.Schedule;

public class BatchProcessScheduledTask : RecurringTask<BatchProcessScheduledTask>
{
    private readonly int _interval;
    private readonly ILogger<BatchProcessScheduledTask> _logger;
    private readonly IBatchService _service;

    public BatchProcessScheduledTask(ILogger<BatchProcessScheduledTask> logger, IBatchService service, IOptions<GeneralConfiguration> generalConfiguration)
    {
        _logger = logger;
        _service = service;
        _interval = generalConfiguration.Value?.BatchProcessIntervals?.Default ?? 1;
    }

    public override string Name => "batchprocess";

    public override string Schedule => $"*/{_interval} * * * *"; //execute every X min

    public override async Task Handle(PerformContext context)
    {
        try
        {
            var now = DateTime.Now;
            _logger.LogInformation($"Executing a scheduled task: {Name} @ {now} with interval: {_interval} min, next execution: {now.AddMinutes(_interval)}");
            await _service.HandleAsync(new BatchService.GetPendingBatchRecords(), CancellationToken.None);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogInformation(e, e.Message);
        }
    }
}
