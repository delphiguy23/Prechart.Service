namespace Prechart.Service.Globals.Interfaces.Batch;

public interface IUpdateBatchProgress
{
    string BatchId { get; set; }
    int CompletedTask { get; set; }
    int TotalTask { get; set; }
}
