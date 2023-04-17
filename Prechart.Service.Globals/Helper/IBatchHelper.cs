using Polly;
using Prechart.Service.Globals.Interfaces.Batch;
using Prechart.Service.Globals.Models.Batch;

namespace Prechart.Service.Globals.Helper;

public interface IBatchHelper
{
    Task CreateBatchRecord(BatchProcess batchRecord);
    Task GetPendingBatchRecords(string batchName);
    Task UpdateProgress(string batchId, int completedTask, int totalTask);
    Task UpdateBatchErrors(string batchId, List<string> errors);
    Task UpdateBatchStatus(string batchId, BatchProcessStatus status);
    Task PendingBatchRecordsForProcessing(List<BatchProcess> batchRecords);
    IAsyncPolicy<T> AwaitAndRetry<T>(int retryCount);
    Task PublishBatchScheduleTaskEvent(IBatchScheduledTaskEvent request);
}
