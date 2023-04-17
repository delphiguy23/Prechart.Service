using Prechart.Service.Core.Models;
using System;

namespace Prechart.Service.TimeSheet.Repositories
{
    public partial class TimeSheetRepository : ITimeSheetRepository
    {
        public record TimeRecord
        {
            public int Id { get; set; }
            public DateTime RecordDate { get; set; }
            public TimeRecordType RecordType { get; set; }
        }

        public record GetTimeRecord
        {
            public int Id { get; set; }
            public DateTime RecordDate { get; set; }
        }
    }
}
