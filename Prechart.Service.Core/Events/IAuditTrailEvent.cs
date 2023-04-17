using System;

namespace Prechart.Service.Core.Events;

public interface IAuditTrailEvent
{
    DateTime LogDate { get; set; }
    string User { get; set; }
    string Table { get; set; }
    int TableId { get; set; }
    string Action { get; set; }
    string Changes { get; set; }
    string AdditionalLogs { get; set; }
}