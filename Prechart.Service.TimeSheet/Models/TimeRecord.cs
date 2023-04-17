using System;

namespace Prechart.Service.TimeSheet.Models
{
    public class TimeRecord
    {
        public int Id { get; set; }
        public DateTime RecordDate { get; set; }
        public int RecordType { get; set; }
    }
}
