using Newtonsoft.Json.Converters;
using Prechart.Service.Globals.Models.Xsd.Loonheffings;
using System.Text.Json.Serialization;

namespace Prechart.Service.Globals.Models.Loonheffings;

public class TaxPaymentDetails
{
    public string TaxNo { get; set; }
    public string NumIv { get; set; }
    public string PersonNr { get; set; }
    public DateTime TaxFileProcessDate { get; set; }
    public string TaxPeriode { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public CollectieveType CollectieveType { get; set; }
    public List<InkomstenPeriodeModel> Inkomstenperiode { get; set; }
    public WerknemersgegevensType Werknemersgegevens { get; set; }
}
