using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Prechart.Service.AuditLog.Service;
using Prechart.Service.Globals.Interfaces.Batch;

namespace Prechart.Service.AuditLog.Consumer;

public class PostLogsToDbConsumer : IConsumer<IBatchScheduledTaskEvent>
{
    private readonly ILogger<PostLogsToDbConsumer> _logger;
    private readonly IAuditLogService _service;

    public PostLogsToDbConsumer(ILogger<PostLogsToDbConsumer> logger, IAuditLogService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task Consume(ConsumeContext<IBatchScheduledTaskEvent> context)
    {
        try
        {
            if (IsValidMessage(context))
            {
                await _service.HandleAsync(new AuditLogService.PostAuditLogToMongo(), CancellationToken.None);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(Consume));
        }
    }

    private static bool IsValidMessage(ConsumeContext<IBatchScheduledTaskEvent> context)
    {
        if (context?.Message is null || context?.Message is not IBatchScheduledTaskEvent)
        {
            return false;
        }

        return context.Message.ScheduledTaskName == "batchpostlogstomongodb" &&
               context.Message.ScheduledTaskEvent == "batchpostlogstomongodb";
    }
}
