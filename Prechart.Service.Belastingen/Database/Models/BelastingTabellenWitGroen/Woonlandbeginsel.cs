using Prechart.Service.Globals.Interfaces.Belastingen;

namespace Prechart.Service.Belastingen.Database.Models;

public class Woonlandbeginsel : IWoonlandbeginsel
{
    public int Id { get; set; }
    public string WoonlandbeginselCode { get; set; }
    public string WoonlandbeginselBenaming { get; set; }
    public int WoonlandbeginselBelastingCode { get; set; }
    public bool Active { get; set; }
}