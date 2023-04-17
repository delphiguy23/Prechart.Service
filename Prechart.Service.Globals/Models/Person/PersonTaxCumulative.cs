using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Prechart.Service.Globals.Models.Person;

public class PersonTaxCumulative
{
    public string SofiNr { get; set; }

    public string PersonNr { get; set; }

    public string NumIv { get; set; }

    public string TaxNo { get; set; }

    public DateTime TaxFileProcessDate { get; set; }

    public string TaxPeriode { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal LnLbPh { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal LnSv { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PrlnAofAnwLg { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PrlnAofAnwHg { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PrlnAofAnwUit { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PrlnAwfAnwLg { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PrlnAwfAnwHg { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PrlnAwfAnwHz { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PrLnUfo { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal LnTabBb { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal VakBsl { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal OpgRchtVakBsl { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal OpnAvwb { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal OpbAvwb { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal LnInGld { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal WrdLn { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal LnOwrk { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal VerstrAanv { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal IngLbPh { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PrAofLg { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PrAofHg { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PrAofUit { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal OpslWko { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PrGediffWhk { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PrAwfLg { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PrAwfHg { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PrAwfHz { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PrUfo { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal BijdrZvw { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal WghZvw { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal WrdPrGebrAut { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal WrknBijdrAut { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Reisk { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal VerrArbKrt { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal AantVerlU { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Ctrctln { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal AantCtrcturenPWk { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal BedrRntKstvPersl { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal BedrAlInWwb { get; set; }

    [BsonRepresentation(BsonType.Decimal128)]
    public decimal BedrRchtAl { get; set; }
}
