namespace Prechart.Service.Globals.Models.Batch;

public enum BatchProcessStatus
{
    Cancelled = -1,
    ReadyForProcessing,
    CurrentlyProcessing,
    CompletedSuccessful,
    CompletedWithFailure,
}
