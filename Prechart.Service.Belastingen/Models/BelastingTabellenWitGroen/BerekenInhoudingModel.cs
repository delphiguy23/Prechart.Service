using Prechart.Service.Globals.Models.Belastingen;

namespace Prechart.Service.Belastingen.Models;

public class BerekenInhoudingModel : IBerekenenInhouding
{
    public decimal InhoudingWit { get; set; }
    public decimal InhoudingGroen { get; set; }
    public decimal BasisDagen { get; set; }
    public decimal AlgemeneHeffingsKorting { get; set; }
    public bool AlgemeneHeffingsKortingIndicator { get; set; }
    public decimal ArbeidsKorting { get; set; }
    public int Loontijdvak { get; set; }
    public int WoonlandbeginselId { get; set; }
    public TaxRecordType InhoudingType { get; set; }
    public string WoonlandbeginselNaam { get; set; }
    public decimal NettoBetaling { get; set; }
}