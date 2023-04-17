using Prechart.Service.Globals.Models.Batch;

namespace Prechart.Service.Globals.Interfaces.Batch;

public interface IPendingBatchRecords
{    
    BatchProcess BatchProcess { get; set; }
}
