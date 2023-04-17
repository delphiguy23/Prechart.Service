namespace Prechart.Service.Globals.Interfaces.Belastingen;

public interface IInsertToTaxResultMessage
{
    public List<IInsertToTaxResult> InsertToTaxResult { get; set; }
    public string Message { get; set; }
}