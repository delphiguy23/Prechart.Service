using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;
using Prechart.Service.Globals.Models.Loonheffings;

namespace Prechart.Service.Globals.Models.Person.Daywages;

public class TaxPaymentDaywageDetails
{
    public string TaxNo { get; set; }
    public string NumIv { get; set; }
    public string PersonNr { get; set; }
    public DateTime TaxFileProcessDate { get; set; }
    public string TaxPeriode { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public CollectieveType CollectieveType { get; set; }

    public DaywageWerknemersgegevensType Werknemersgegevens { get; set; }
}
