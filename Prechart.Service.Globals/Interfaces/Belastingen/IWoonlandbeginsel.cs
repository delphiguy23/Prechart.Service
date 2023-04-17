namespace Prechart.Service.Globals.Interfaces.Belastingen;

public interface IWoonlandbeginsel
{
    public int Id { get; set; }
    public string WoonlandbeginselCode { get; set; }
    public string WoonlandbeginselBenaming { get; set; }
    public int WoonlandbeginselBelastingCode { get; set; }
    public bool Active { get; set; }
}