using System.Collections.Generic;
using Prechart.Service.Globals.Interfaces.Batch;
using Prechart.Service.Globals.Models.Batch;

namespace Prechart.Service.Batch.Services;

public partial class BatchService
{
    public record CreateBatchRecord
    {
        public BatchProcess BatchRecord { get; set; }
    }

    public record GetPendingBatchRecords
    {
        public string BatchName { get; set; }
    }

    public record UpdateProgress
    {
        public string BatchId { get; set; }
        public int CompletedTask { get; set; }
        public int TotalTask { get; set; }
    }

    public record UpdateBatchErrors
    {
        public string BatchId { get; set; }
        public List<string> Errors { get; set; }
    }

    public record UpdateStatus
    {
        public string BatchId { get; set; }
        public BatchProcessStatus Status { get; set; }
    }

    public record PublishBatchScheduleTaskEvent : IBatchScheduledTaskEvent
    {
        public string ScheduledTaskName { get; set; }
        public string ScheduledTaskEvent { get; set; }
    }
}
