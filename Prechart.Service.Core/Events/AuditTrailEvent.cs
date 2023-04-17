using System;

namespace Prechart.Service.Core.Events;

public class AuditTrailEvent : IAuditTrailEvent
{
    public DateTime LogDate { get; set; }
    public string User { get; set; }
    public string Table { get; set; }
    public int TableId { get; set; }
    public string Action { get; set; }
    public string Changes { get; set; }
    public string AdditionalLogs { get; set; }
}