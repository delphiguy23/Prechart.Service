using Prechart.Service.Globals.Models.Belastingen;

namespace Prechart.Service.Belastingen.Models;

public class GetTaxRecord
{
    public TaxRecordType TaxType { get; set; }
    public int Year { get; set; }
    public int WoonlandbeginselId { get; set; }
    public int TypeId { get; set; }
    public decimal Amount { get; set; }
}