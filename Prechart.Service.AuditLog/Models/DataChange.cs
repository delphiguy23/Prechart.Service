namespace Prechart.Service.AuditLog.Models;

public class DataChange
{
    public string ColumnName { get; set; }
    public string OldValue { get; set; }
    public string NewValue { get; set; }
}