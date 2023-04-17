namespace Prechart.Service.Globals.Interfaces.Batch;

public interface IUpdateBatchErrors
{
    string BatchId { get; set; }
    List<string> Errors { get; set; }
}
