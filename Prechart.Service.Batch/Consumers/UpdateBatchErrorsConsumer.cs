using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Prechart.Service.Batch.Services;
using Prechart.Service.Globals.Interfaces.Batch;

namespace Prechart.Service.Batch.Consumers;

public class UpdateBatchErrorsConsumer : IConsumer<IUpdateBatchErrors>
{
    private readonly ILogger<UpdateBatchErrorsConsumer> _logger;
    private readonly IBatchService _service;

    public UpdateBatchErrorsConsumer(ILogger<UpdateBatchErrorsConsumer> logger, IBatchService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task Consume(ConsumeContext<IUpdateBatchErrors> context)
    {
        _logger.LogInformation($"{nameof(IUpdateBatchErrors)} event with payload {context.Message.ToJson()}");

        try
        {
            if (context.Message is IUpdateBatchErrors)
            {
                await _service.HandleAsync(new BatchService.UpdateBatchErrors
                {
                    BatchId = context.Message.BatchId,
                    Errors = context.Message.Errors,
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
