using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Prechart.Service.AuditLog.Repositories;
using Prechart.Service.AuditLog.Utils;

namespace Prechart.Service.AuditLog.Consumer;

public class ControllerLogsEventConsumer : IConsumer<IControllerLogs>
{
    private readonly ILogger<ControllerLogsEventConsumer> _logger;
    private readonly IAuditLogRepository _repository;

    public ControllerLogsEventConsumer(ILogger<ControllerLogsEventConsumer> logger, IAuditLogRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<IControllerLogs> context)
    {
        try
        {
            var trail = await _repository.HandleAsync(new AuditLogRepository.LogControllerActions
            {
                ControllerLogs = context.Message,
            }, CancellationToken.None);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(Consume));
        }
    }
}
