using Prechart.Service.Globals.Interfaces.Belastingen;

namespace Prechart.Service.Documents.Upload.Csv.Models;

public class TaxResult : IInsertToTaxResult
{
    public string Filename { get; set; }
    public bool IsProcessed { get; set; }
}

