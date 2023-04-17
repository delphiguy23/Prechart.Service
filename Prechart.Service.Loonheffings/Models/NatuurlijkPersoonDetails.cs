using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Prechart.Service.Globals.Models.Loonheffings;
using Prechart.Service.Globals.Models.Loonheffings.Xsd.Xsd2022;

namespace Prechart.Service.Loonheffings.Models;

[BsonIgnoreExtraElements]
public class NatuurlijkPersoonDetails
{
    public string NumIV { get; set; }
    public string PersNr { get; set; }
    public InkomstenverhoudingInitieelTypeNatuurlijkPersoon NatuurlijkPersoon { get; set; }
    public WerknemersgegevensType Werknemersgegevens { get; set; }
    public List<InkomstenperiodeInitieelType> Inkomstenperiode { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public CollectieveType CollectieveType { get; set; }
}
