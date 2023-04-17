using Prechart.Service.Globals.Models.Batch;

namespace Prechart.Service.Globals.Interfaces.Batch;

public interface IUpdateBatchStatus
{
    string BatchId { get; set; }
    BatchProcessStatus Status { get; set; }
}
