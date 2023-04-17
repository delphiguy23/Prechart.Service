using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Prechart.Service.Globals.Models.Loonheffings.Enums;

namespace Prechart.Service.Globals.Models.Xsd.Loonheffings;

[BsonIgnoreExtraElements]
public class InkomstenPeriodeModel
{
    public DateTime DatAanv { get; set; }
    public string SrtIv { get; set; }
    public string CdAard { get; set; }
    public string CdInvlVpl { get; set; }
    public string FsIndFz { get; set; }
    public string Cao { get; set; }
    public string CdCaoInl { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Indicatie IndArbovOnbepTd { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Indicatie IndSchriftArbov { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Indicatie IndOprov { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public IndicatieJ IndJrurennrm { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Indicatie IndPubAanOnbepTd { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Indicatie IndAvrLkvOudrWn { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Indicatie IndAvrLkvAgWn { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Indicatie IndAvrLkvDgBafSb { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Indicatie IndAvrLkvHpAgWn { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Indicatie IndLhKort { get; set; }

    public string CdRdnGnBijt { get; set; }

    public string LbTab { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Indicatie IndWao { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Indicatie IndWw { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Indicatie IndZw { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Indicatie IndWgldOudRegl { get; set; }

    public string CdZvw { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Indicatie IndVakBn { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Indicatie IndSa71 { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Indicatie IndSa72 { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Indicatie IndSa03 { get; set; }

    public string CdIncInkVerm { get; set; }
}
