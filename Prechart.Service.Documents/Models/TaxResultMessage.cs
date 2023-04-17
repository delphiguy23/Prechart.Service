using System.Collections.Generic;
using Prechart.Service.Globals.Interfaces.Belastingen;

namespace Prechart.Service.Documents.Upload.Csv.Models;

public class TaxResultMessage : IInsertToTaxResultMessage
{
    public List<IInsertToTaxResult> InsertToTaxResult { get; set; }
    public string Message { get; set; }
}
