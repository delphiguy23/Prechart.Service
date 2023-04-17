using MassTransit;
using Microsoft.Extensions.Logging;
using Prechart.Service.AuditLog.Events.Users;
using Prechart.Service.AuditLog.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Prechart.Service.AuditLog.Consumer;

public class UserEventConsumer : IConsumer<IUserEvent>
{
    private readonly ILogger<UserEventConsumer> _logger;
    private readonly IAuditLogRepository _repository;

    public UserEventConsumer(ILogger<UserEventConsumer> logger, IAuditLogRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<IUserEvent> context)
    {
        try
        {
            var trail = await _repository.HandleAsync(new AuditLogRepository.InsertAuditLogInMemory
            {
                UserName = context.Message.UserName,
                Updated = default,
                TableName = "AspNetUsers",
                PropertyName = null,
                EntityId = int.TryParse(context.Message.UserId, out var id) ? id : 0,
                EntityIdAdditional = 0,
                Action = context.Message.EventType.ToString(),
                Changes = null,
            }, CancellationToken.None
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, nameof(Consume));
        }
    }
}
