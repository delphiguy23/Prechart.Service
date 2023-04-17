using Prechart.Service.Globals.Interfaces.Batch;

namespace Prechart.Service.Globals.Models.Batch;

public class BatchScheduledTaskEvent : IBatchScheduledTaskEvent
{
    public string ScheduledTaskName { get; set; }
    public string ScheduledTaskEvent { get; set; }
}
