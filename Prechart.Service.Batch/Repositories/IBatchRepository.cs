using System.Collections.Generic;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Service;
using Prechart.Service.Globals.Models.Batch;

namespace Prechart.Service.Batch.Repositories;

public interface IBatchRepository :
    IHandlerAsync<BatchRepository.CreateBatchRecord, IFluentResults<bool>>,
    IHandlerAsync<BatchRepository.GetPendingBatchRecords, IFluentResults<List<BatchProcess>>>,
    IHandlerAsync<BatchRepository.UpdateProgress, IFluentResults<bool>>,
    IHandlerAsync<BatchRepository.UpdateBatchErrors, IFluentResults<bool>>,
    IHandlerAsync<BatchRepository.UpdateStatus, IFluentResults<bool>>
{
}
