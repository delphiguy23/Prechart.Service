using Prechart.Service.Globals.Interfaces.Batch;

namespace Prechart.Service.Globals.Models.Batch;

public class GetPendingBatchRecords : IGetPendingBatchRecords
{
    public string BatchName { get; set; }
}
