using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Prechart.Service.Batch.Services;
using Prechart.Service.Globals.Interfaces.Batch;

namespace Prechart.Service.Batch.Consumers;

public class UpdateBatchStatusConsumer : IConsumer<IUpdateBatchStatus>
{
    private readonly ILogger<UpdateBatchStatusConsumer> _logger;
    private readonly IBatchService _service;

    public UpdateBatchStatusConsumer(ILogger<UpdateBatchStatusConsumer> logger, IBatchService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task Consume(ConsumeContext<IUpdateBatchStatus> context)
    {
        _logger.LogInformation($"{nameof(IUpdateBatchStatus)} event with payload {context.Message}");

        try
        {
            if (context.Message is IUpdateBatchStatus)
            {
                await _service.HandleAsync(new BatchService.UpdateStatus
                {
                    BatchId = context.Message.BatchId,
                    Status = context.Message.Status,
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
