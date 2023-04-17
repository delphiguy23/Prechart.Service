using MassTransit;
using Microsoft.Extensions.Logging;
using Prechart.Service.AuditLog.Repositories;
using Prechart.Service.Core.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Prechart.Service.AuditLog.Consumers;

public class GetAuditTraceConsumer : IConsumer<IGetAuditTrail>
{
    private readonly ILogger<GetAuditTraceConsumer> _logger;
    private readonly IAuditLogRepository _repository;

    public GetAuditTraceConsumer(ILogger<GetAuditTraceConsumer> logger, IAuditLogRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<IGetAuditTrail> context)
    {
        try
        {
            var trail = await _repository.HandleAsync(new AuditLogRepository.GetAuditTrail
            {
                TableName = context.Message?.TableName,
                TableId = context.Message?.TableId,
            }, CancellationToken.None);

            await context.RespondAsync<IAuditTrails>(trail.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(Consume));
        }
    }
}
