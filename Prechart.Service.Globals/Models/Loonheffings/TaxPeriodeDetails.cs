using Prechart.Service.Globals.Models.Xsd.Loonheffings;

namespace Prechart.Service.Globals.Models.Loonheffings;

public class TaxPeriodeDetails
{
    public DateTime TaxFileProcessDate { get; set; }
    public string TaxPeriode { get; set; }
    public List<InkomstenPeriodeModel> Inkomstenperiode { get; set; }
    public WerknemersgegevensType Werknemersgegevens { get; set; }
}
