using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Prechart.Service.Batch.Repositories;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Globals.Helper;

namespace Prechart.Service.Batch.Services;

public partial class BatchService : IBatchService
{
    private readonly IBatchHelper _batchHelper;
    private readonly ILogger<BatchService> _logger;
    private readonly IBatchRepository _repository;

    public BatchService(ILogger<BatchService> logger, IBatchRepository repository, IBatchHelper batchHelper)
    {
        _logger = logger;
        _repository = repository;
        _batchHelper = batchHelper;
    }

    public async Task<IFluentResults<bool>> HandleAsync(UpdateProgress request, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _repository.HandleAsync(new BatchRepository.UpdateProgress
            {
                BatchId = request.BatchId,
                CompletedTask = request.CompletedTask,
                TotalTask = request.TotalTask,
            }, cancellationToken);

            return ResultsTo.Success<bool>().FromResults(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<bool>().FromException(e);
        }
    }

    public async Task<IFluentResults<bool>> HandleAsync(UpdateBatchErrors request, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _repository.HandleAsync(new BatchRepository.UpdateBatchErrors
            {
                BatchId = request.BatchId,
                Errors = request.Errors,
            }, cancellationToken);

            return ResultsTo.Something(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<bool>().FromException(e);
        }
    }

    public async Task<IFluentResults<bool>> HandleAsync(UpdateStatus request, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _repository.HandleAsync(new BatchRepository.UpdateStatus
            {
                BatchId = request.BatchId,
                Status = request.Status,
            }, cancellationToken);

            return ResultsTo.Success<bool>().FromResults(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<bool>().FromException(e);
        }
    }

    public async Task<IFluentResults<bool>> HandleAsync(GetPendingBatchRecords request, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _repository.HandleAsync(new BatchRepository.GetPendingBatchRecords
            {
                BatchName = request.BatchName,
            }, cancellationToken);

            if ((result?.Value ?? null) is null || !result.Value.Any())
            {
                return ResultsTo.NotFound<bool>().WithMessage("No Pending Batch Records Found.");
            }

            await _batchHelper.PendingBatchRecordsForProcessing(result.Value);

            return ResultsTo.Success<bool>().FromResults(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<bool>().FromException(e);
        }
    }

    public async Task<IFluentResults> HandleAsync(PublishBatchScheduleTaskEvent request, CancellationToken cancellationToken = default)
    {
        await _batchHelper.PublishBatchScheduleTaskEvent(request);
        return ResultsTo.Success();
    }

    public async Task<IFluentResults<bool>> HandleAsync(CreateBatchRecord request, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _repository.HandleAsync(new BatchRepository.CreateBatchRecord
            {
                BatchRecord = request.BatchRecord,
            }, cancellationToken);

            return ResultsTo.Something(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<bool>().FromException(e);
        }
    }
}
