using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Service;

namespace Prechart.Service.Batch.Services;

public interface IBatchService :
    IHandlerAsync<BatchService.CreateBatchRecord, IFluentResults<bool>>,
    IHandlerAsync<BatchService.GetPendingBatchRecords, IFluentResults<bool>>,
    IHandlerAsync<BatchService.UpdateProgress, IFluentResults<bool>>,
    IHandlerAsync<BatchService.UpdateBatchErrors, IFluentResults<bool>>,
    IHandlerAsync<BatchService.UpdateStatus, IFluentResults<bool>>,
    IHandlerAsync<BatchService.PublishBatchScheduleTaskEvent, IFluentResults>
{
}