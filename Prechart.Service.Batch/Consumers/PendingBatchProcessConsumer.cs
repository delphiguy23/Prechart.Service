using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Prechart.Service.Batch.Services;
using Prechart.Service.Globals.Interfaces.Batch;

namespace Prechart.Service.Batch.Consumers;

public class PendingBatchProcessConsumer : IConsumer<IGetPendingBatchRecords>
{
    private readonly ILogger<PendingBatchProcessConsumer> _logger;
    private readonly IBatchService _service;

    public PendingBatchProcessConsumer(ILogger<PendingBatchProcessConsumer> logger, IBatchService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task Consume(ConsumeContext<IGetPendingBatchRecords> context)
    {
        _logger.LogInformation($"{nameof(IGetPendingBatchRecords)} event with payload {context.Message.ToJson()}");

        try
        {
            if (context.Message is IGetPendingBatchRecords)
            {
                await _service.HandleAsync(new BatchService.GetPendingBatchRecords
                {
                    BatchName = context.Message.BatchName,
                }, CancellationToken.None);

                _logger.LogInformation(context.Message.ToJson());
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(Consume));
        }
    }
}
