using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Prechart.Service.Batch.Services;
using Prechart.Service.Globals.Interfaces.Batch;
using Prechart.Service.Globals.Models.Batch;

namespace Prechart.Service.Batch.Consumers;

public class CreateBatchRecordConsumer : IConsumer<IBatchProcessModel>
{
    private readonly ILogger<CreateBatchRecordConsumer> _logger;
    private readonly IBatchService _service;

    public CreateBatchRecordConsumer(ILogger<CreateBatchRecordConsumer> logger, IBatchService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task Consume(ConsumeContext<IBatchProcessModel> context)
    {
        if (context.Message is not null && context.Message is IBatchProcessModel)
        {
            _logger.LogInformation($"{nameof(IBatchProcessModel)} event with payload {context.Message.ToJson()}");

            var message = new BatchProcess
            {
                BatchName = context.Message.BatchName,
                TotalTask = context.Message.TotalTask,
                CompletedTask = context.Message.CompletedTask,
                Status = BatchProcessStatus.ReadyForProcessing,
                PublishedOn = context.Message.PublishedOn,
                ScheduledOn = context.Message.ScheduledOn,
                StartedOn = context.Message.StartedOn,
                FinalizedOn = context.Message.FinalizedOn,
                Payload = context.Message.Payload,
                BatchErrors = context.Message.BatchErrors ?? new List<string>(),
            };

            await _service.HandleAsync(new BatchService.CreateBatchRecord
            {
                BatchRecord = message,
            }, CancellationToken.None);
        }
    }
}
