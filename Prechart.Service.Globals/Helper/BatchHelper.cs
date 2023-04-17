using MassTransit;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Prechart.Service.Globals.Interfaces.Batch;
using Prechart.Service.Globals.Models.Batch;

namespace Prechart.Service.Globals.Helper;

public class BatchHelper : IBatchHelper
{
    private readonly ILogger<BatchHelper> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public BatchHelper(ILogger<BatchHelper> logger, IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    public async Task CreateBatchRecord(BatchProcess batchRecord)
    {
        await _publishEndpoint.Publish<IBatchProcessModel>(batchRecord);
        _logger.LogInformation("New BatchRecord Created");
    }

    public async Task GetPendingBatchRecords(string batchName)
    {
        await _publishEndpoint.Publish<IGetPendingBatchRecords>(new {BatchName = batchName});
        _logger.LogInformation("Get Pending BatchRecords Triggered");
    }

    public async Task UpdateBatchStatus(string batchId, BatchProcessStatus status)
    {
        await _publishEndpoint.Publish<IUpdateBatchStatus>(new
        {
            BatchId = batchId,
            Status = status,
        });
        _logger.LogInformation($"Update BatchRecord Status Triggered, Id [{batchId}]");
    }

    public async Task UpdateProgress(string batchId, int completedTask, int totalTask)
    {
        await _publishEndpoint.Publish<IUpdateBatchProgress>(new
        {
            BatchId = batchId,
            CompletedTask = completedTask,
            TotalTask = totalTask,
        });
        _logger.LogInformation($"Update BatchRecord Triggered, Id [{batchId}]");
    }

    public async Task UpdateBatchErrors(string batchId, List<string> errors)
    {
        await _publishEndpoint.Publish<IUpdateBatchErrors>(new
        {
            BatchId = batchId,
            Errors = errors,
        });
        _logger.LogInformation($"BatchRecord Errors Triggered, Id [{batchId}]");
    }

    public async Task PendingBatchRecordsForProcessing(List<BatchProcess> batchRecords)
    {
        foreach (var batchProcess in batchRecords)
        {
            await _publishEndpoint.Publish<IPendingBatchRecords>(new
            {
                BatchProcess = batchProcess,
            });
            _logger.LogInformation("Pending BatchRecord Published, BatchName [{BatchProcessBatchName}]", batchProcess.BatchName);
        }
    }

    public IAsyncPolicy<T> AwaitAndRetry<T>(int retryCount = 5)
    {
        var retryPolicy =
            Policy<T>
                .Handle<MongoException>()
                .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2
                    (
                        TimeSpan.FromSeconds(1),
                        retryCount
                    )
                );

        return retryPolicy;
    }

    public async Task PublishBatchScheduleTaskEvent(IBatchScheduledTaskEvent request)
    {
        await _publishEndpoint.Publish<IBatchScheduledTaskEvent>(new BatchScheduledTaskEvent
        {
            ScheduledTaskName = request.ScheduledTaskName,
            ScheduledTaskEvent = request.ScheduledTaskEvent,
        });
    }
}
