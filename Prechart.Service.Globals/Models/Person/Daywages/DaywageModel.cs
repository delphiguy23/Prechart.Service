using Prechart.Service.Globals.Interfaces.Person.Daywage;

namespace Prechart.Service.Globals.Models.Person.Daywages;

public class DaywageModel : IDaywage
{
    public DateTime StartOfDaywage { get; set; }
    public DateTime EndOfDaywage { get; set; }
    public decimal Daywage { get; set; }
    public bool Active { get; set; }
}
