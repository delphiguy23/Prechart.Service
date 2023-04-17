namespace Prechart.Service.Globals.Interfaces.Belastingen;

public interface IInsertToTaxResult
{
    public string Filename { get; set; }
    public bool IsProcessed { get; set; }
}