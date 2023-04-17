using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Prechart.Service.Globals.Models.Loonheffings;

public class SaldoCorrectiesVoorgaandTijdvakModel
{
    public DateTime DatAanvTv { get; set; }
    public DateTime DatEindTv { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Saldo { get; set; }
}
