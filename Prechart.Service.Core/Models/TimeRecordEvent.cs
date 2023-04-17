using System;

namespace Prechart.Service.Core.Models;

public class TimeRecordEvent
{
    public int EmployeeId { get; set; }
    public DateTime RecordDate { get; set; }
    public TimeRecordType RecordType { get; set; }
}