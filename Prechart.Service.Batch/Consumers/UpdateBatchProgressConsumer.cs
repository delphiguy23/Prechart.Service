using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Prechart.Service.Batch.Services;
using Prechart.Service.Globals.Interfaces.Batch;

namespace Prechart.Service.Batch.Consumers;

public class UpdateBatchProgressConsumer : IConsumer<IUpdateBatchProgress>
{
    private readonly ILogger<UpdateBatchProgressConsumer> _logger;
    private readonly IBatchService _service;

    public UpdateBatchProgressConsumer(ILogger<UpdateBatchProgressConsumer> logger, IBatchService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task Consume(ConsumeContext<IUpdateBatchProgress> context)
    {
        _logger.LogInformation($"{nameof(IUpdateBatchProgress)} event with payload {context.Message}");

        try
        {
            if (context.Message is IUpdateBatchProgress)
            {
                await _service.HandleAsync(new BatchService.UpdateProgress
                {
                    BatchId = context.Message.BatchId,
                    CompletedTask = context.Message.CompletedTask,
                    TotalTask = context.Message.TotalTask,
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
