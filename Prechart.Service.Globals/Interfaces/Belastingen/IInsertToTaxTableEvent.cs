namespace Prechart.Service.Globals.Interfaces.Belastingen;

public interface IInsertToTaxTableEvent
{
    public string TaxType { get; set; }
    public List<ITaxRecord> TaxTable { get; set; }
}