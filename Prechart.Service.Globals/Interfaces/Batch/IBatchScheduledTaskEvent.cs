namespace Prechart.Service.Globals.Interfaces.Batch;

public interface IBatchScheduledTaskEvent
{
    string ScheduledTaskName { get; }
    string ScheduledTaskEvent { get; }
}
