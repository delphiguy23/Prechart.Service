using Prechart.Service.Belastingen.Models.PremiePercentagesSocialeVerzekeringen;
using Prechart.Service.Belastingen.Models.Werkgever;

namespace Prechart.Service.Belastingen.Models.Berekeningen;

public class GetBerekenen
{
    public GetInhouding InhoudingParameters { get; set; }
    public GetPremieBedrag PremieBedragParameters { get; set; }
    public GetWerkgever WhkWerkgeverParameters { get; set; }
    
    public string KlantId { get; set; }
    public string EmployeeId { get; set; }
    
    public HighLowEnum PremieBedragAlgemeenWerkloosheIdsFondsLaagHoog { get; set; }
    public bool IsPremieBedragUitvoeringsFondsvoordeOverheId { get; set; }
    public HighLowEnum PremieBedragWetArbeIdsOngeschikheIdLaagHoog { get; set; }
    public PayeeEnum Payee { get; set; }
}