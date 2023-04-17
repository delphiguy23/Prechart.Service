using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Prechart.Service.Globals.Models.Loonheffings;

[BsonIgnoreExtraElements]
public class CollectieveAangifteModel
{
    public string TaxNo { get; set; }
    public string Periode { get; set; }
    public DateTime ProcessedDate { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public CollectieveType CollectieveType { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal TotLnLbPh { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal TotLnSv { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal TotPrlnAofAnwLg { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal TotPrlnAofAnwHg { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal TotPrlnAofAnwUit { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal TotPrlnAwfAnwLg { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal TotPrlnAwfAnwHg { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal TotPrlnAwfAnwHz { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PrLnUfo { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal IngLbPh { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal EhPubUitk { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal EhGebrAuto { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal EhVut { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal EhOvsFrfWrkkstrg { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal AvZeev { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal VrlAvso { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal TotPrAofLg { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal TotPrAofHg { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal TotPrAofUit { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal TotOpslWko { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal TotPrGediffWhk { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal TotPrAwfLg { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal TotPrAwfHg { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal TotPrAwfHz { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PrUfo { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal IngBijdrZvw { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal TotWghZvw { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal TotTeBet { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal TotGen { get; set; }
    
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal TotSaldo { get; set; }

    public SaldoCorrectiesVoorgaandTijdvakModel[] SaldoCorrectiesVoorgaandTijdvak { get; set; }
}
