using System;
using System.ComponentModel.DataAnnotations;

namespace Prechart.Service.AuditLog.Database.Models;

public class AuditLog
{
    [Key] public int Id { get; set; }
    public DateTime LogDate { get; set; }
    public string User { get; set; }
    public string Table { get; set; }
    public int TableId { get; set; }
    public string Action { get; set; }
    public string Changes { get; set; }
    public string AdditionalLogs { get; set; }
}