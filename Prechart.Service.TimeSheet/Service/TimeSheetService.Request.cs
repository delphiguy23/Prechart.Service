using System;
using Prechart.Service.Core.Models;

namespace Prechart.Service.TimeSheet.Service;

public partial class TimeSheetService
{
    public record InsertTimeRecord
    {
        public int Id { get; set; }
        public DateTime RecordDate { get; set; }
        public TimeRecordType RecordType { get; set; }
    }
}