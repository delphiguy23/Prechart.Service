using Prechart.Service.AuditLog.Models.Users;

namespace Prechart.Service.AuditLog.Events.Users;

public interface IUserEvent
{
    public UserEventType EventType { get; set; }
    public string UserName { get; set; }
    public string UserId { get; set; }
}