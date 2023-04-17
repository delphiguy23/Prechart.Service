using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Server;
using Microsoft.Extensions.Logging;
using Prechart.Service.Batch.Services;
using Prechart.Service.Core.Scheduling;

namespace Prechart.Service.Batch.Schedule;

public class BatchPostLogsTask : RecurringTask<BatchPostLogsTask>
{
    private readonly ILogger<BatchPostLogsTask> _logger;
    private readonly IBatchService _service;

    public BatchPostLogsTask(ILogger<BatchPostLogsTask> logger, IBatchService service)
    {
        _logger = logger;
        _service = service;
    }

    public override string Name => "batchpostlogstomongodb";

    public override string Schedule => Cron.Minutely();

    public override async Task Handle(PerformContext context)
    {
        await _service.HandleAsync(new BatchService.PublishBatchScheduleTaskEvent
        {
            ScheduledTaskEvent = "batchpostlogstomongodb",
            ScheduledTaskName = "batchpostlogstomongodb",
        }, CancellationToken.None);
    }
}
