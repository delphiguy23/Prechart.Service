using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Prechart.Service.Core.Extensions;
using Prechart.Service.Core.FluentResults;
using Prechart.Service.Core.Outcomes;
using Prechart.Service.Globals.Models.Batch;

namespace Prechart.Service.Batch.Repositories;

public partial class BatchRepository : IBatchRepository
{
    private readonly IMongoCollection<BatchProcess> _batchProcess;
    private readonly ILogger<BatchRepository> _logger;

    private readonly IAsyncPolicy<BatchProcess> _retryPolicy =
        Policy<BatchProcess>
            .Handle<MongoException>()
            .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 5));

    private readonly IAsyncPolicy _retryPolicyInsert =
        Policy
            .Handle<MongoException>()
            .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 5));

    private readonly IAsyncPolicy<List<BatchProcess>> _retryPolicyList =
        Policy<List<BatchProcess>>
            .Handle<MongoException>()
            .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 5));

    private readonly IAsyncPolicy<UpdateResult> _retryPolicyUpdate =
        Policy<UpdateResult>
            .Handle<MongoException>()
            .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 5));

    public BatchRepository(ILogger<BatchRepository> logger, IMongoCollection<BatchProcess> batchProcess)
    {
        _logger = logger;
        _batchProcess = batchProcess;
    }

    public async Task<IFluentResults<bool>> HandleAsync(CreateBatchRecord request, CancellationToken cancellationToken = default)
    {
        try
        {
            if (request.BatchRecord is not null && request.BatchRecord is BatchProcess)
            {
                await _retryPolicyInsert.ExecuteAsync(async () => await _batchProcess.InsertOneAsync(request.BatchRecord, cancellationToken));
                return ResultsTo.Success(true);
            }

            return ResultsTo.Success(false);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<bool>().FromException(e);
        }
    }

    public async Task<IFluentResults<List<BatchProcess>>> HandleAsync(GetPendingBatchRecords request, CancellationToken cancellationToken = default)
    {
        try
        {
            var filter = Builders<BatchProcess>.Filter.Eq(b => b.Status, BatchProcessStatus.ReadyForProcessing);

            if (!string.IsNullOrWhiteSpace(request?.BatchName))
            {
                filter &= Builders<BatchProcess>.Filter.Eq(b => b.BatchName, request.BatchName);
            }

            var result = await _retryPolicyList.ExecuteAsync(async () =>
                await _batchProcess.FindAsync(filter)
                    .Result
                    .ToListAsync());

            return ResultsTo.Success(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<List<BatchProcess>>().FromException(e);
        }
    }

    public async Task<IFluentResults<bool>> HandleAsync(UpdateStatus request, CancellationToken cancellationToken)
    {
        try
        {
            var filter = Builders<BatchProcess>.Filter.Eq(b => b.Id, request.BatchId.ToObjectId());

            switch (request.Status)
            {
                case BatchProcessStatus.CurrentlyProcessing:
                    var updateStartedOn = Builders<BatchProcess>
                        .Update
                        .Set(b => b.Status, request.Status)
                        .Set(b => b.StartedOn, DateTime.Now);

                    await _retryPolicy.ExecuteAsync(async () => await _batchProcess.FindOneAndUpdateAsync(filter, updateStartedOn));

                    break;
                case BatchProcessStatus.CompletedSuccessful:
                case BatchProcessStatus.CompletedWithFailure:
                case BatchProcessStatus.Cancelled:
                    var updateFinalizeOn = Builders<BatchProcess>
                        .Update
                        .Set(b => b.Status, request.Status)
                        .Set(b => b.FinalizedOn, DateTime.Now);

                    await _retryPolicy.ExecuteAsync(async () => await _batchProcess.FindOneAndUpdateAsync(filter, updateFinalizeOn));

                    break;
            }

            return ResultsTo.Success(true);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<bool>().FromException(e);
        }
    }

    public async Task<IFluentResults<bool>> HandleAsync(UpdateProgress request, CancellationToken cancellationToken)
    {
        try
        {
            var filter = Builders<BatchProcess>.Filter.Eq(b => b.Id, request.BatchId.ToObjectId());
            var update = Builders<BatchProcess>.Update
                .Set(b => b.CompletedTask, request.CompletedTask)
                .Set(b => b.TotalTask, request.TotalTask);

            await _retryPolicyUpdate.ExecuteAsync(async () => await _batchProcess.UpdateOneAsync(filter, update));
            return ResultsTo.Success(true);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<bool>().FromException(e);
        }
    }

    public async Task<IFluentResults<bool>> HandleAsync(UpdateBatchErrors request, CancellationToken cancellationToken)
    {
        try
        {
            var filter = Builders<BatchProcess>.Filter.Eq(b => b.Id, request.BatchId.ToObjectId());

            if (request?.Errors is null || !request.Errors.Any())
            {
                return ResultsTo.Success(false);
            }

            foreach (var error in request.Errors)
            {
                var update = Builders<BatchProcess>.Update.Push(b => b.BatchErrors, error);
                await _retryPolicy.ExecuteAsync(async () => await _batchProcess.FindOneAndUpdateAsync(filter, update));
            }

            return ResultsTo.Success(true);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return ResultsTo.Failure<bool>().FromException(e);
        }
    }
}
