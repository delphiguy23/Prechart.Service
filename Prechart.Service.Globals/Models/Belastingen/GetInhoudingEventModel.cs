using Prechart.Service.Globals.Interfaces.Belastingen;

namespace Prechart.Service.Globals.Models.Belastingen;

public class GetInhoudingEventModel : IGetInhouding
{
    public decimal InkomenWit { get; set; }
    public decimal InkomenGroen { get; set; }
    public int BasisDagen { get; set; }
    public DateTime Geboortedatum { get; set; }
    public bool AlgemeneHeffingsKortingIndicator { get; set; }
    public TaxPeriodEnum Loontijdvak { get; set; }
    public int WoondlandBeginselId { get; set; }
    public DateTime ProcesDatum { get; set; }
}