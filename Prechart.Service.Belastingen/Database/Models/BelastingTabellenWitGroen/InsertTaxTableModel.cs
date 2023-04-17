using System.Collections.Generic;
using Prechart.Service.Globals.Interfaces.Belastingen;

namespace Prechart.Service.Belastingen.Database.Models;

public class InsertTaxTableModel : IInsertToTaxTableEvent
{
    public string TaxType { get; set; }
    public List<ITaxRecord> TaxTable { get; set; }
}