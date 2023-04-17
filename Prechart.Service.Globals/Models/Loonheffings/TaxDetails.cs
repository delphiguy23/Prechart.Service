using Prechart.Service.Globals.Models.Xsd.Loonheffings;

namespace Prechart.Service.Globals.Models.Loonheffings;

public class TaxDetails
{
    public List<InkomstenperiodeInitieelType> Inkomstenperiode { get; set; }
    public WerknemersgegevensType Werknemersgegevens { get; set; }
}
