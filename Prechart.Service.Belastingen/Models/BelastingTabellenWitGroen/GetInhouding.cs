using Prechart.Service.Globals.Models.Belastingen;
using System;
using Prechart.Service.Globals.Interfaces.Belastingen;

namespace Prechart.Service.Belastingen.Models;

public class GetInhouding : IGetInhouding
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