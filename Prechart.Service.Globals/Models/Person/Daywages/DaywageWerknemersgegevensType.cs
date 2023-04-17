using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Prechart.Service.Globals.Models.Person.Daywages;

public class DaywageWerknemersgegevensType
{
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal LnSv { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal AantVerlU { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Days => AantVerlU > 0 ? AantVerlU / 8 : 0;
}
