using Prechart.Service.Globals.Interfaces.Batch;

namespace Prechart.Service.Globals.Models.Batch;

public class PendingBatchRecords : IPendingBatchRecords
{
    public BatchProcess BatchProcess { get; set; }
}
