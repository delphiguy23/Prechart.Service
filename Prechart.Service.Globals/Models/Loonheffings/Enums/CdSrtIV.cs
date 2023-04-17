using System.Text.Json.Serialization;
using System.Xml.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Prechart.Service.Globals.Models.Loonheffings.Enums;

public enum CdSrtIV
{
    /// <remarks />
    [XmlEnum("11")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item11,

    /// <remarks />
    [XmlEnum("13")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item13,

    /// <remarks />
    [XmlEnum("15")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item15,

    /// <remarks />
    [XmlEnum("17")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item17,

    /// <remarks />
    [XmlEnum("18")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item18,

    /// <remarks />
    [XmlEnum("22")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item22,

    /// <remarks />
    [XmlEnum("23")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item23,

    /// <remarks />
    [XmlEnum("24")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item24,

    /// <remarks />
    [XmlEnum("31")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item31,

    /// <remarks />
    [XmlEnum("32")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item32,

    /// <remarks />
    [XmlEnum("33")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item33,

    /// <remarks />
    [XmlEnum("34")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item34,

    /// <remarks />
    [XmlEnum("36")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item36,

    /// <remarks />
    [XmlEnum("37")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item37,

    /// <remarks />
    [XmlEnum("38")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item38,

    /// <remarks />
    [XmlEnum("39")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item39,

    /// <remarks />
    [XmlEnum("40")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item40,

    /// <remarks />
    [XmlEnum("42")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item42,

    /// <remarks />
    [XmlEnum("43")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item43,

    /// <remarks />
    [XmlEnum("45")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item45,

    /// <remarks />
    [XmlEnum("46")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item46,

    /// <remarks />
    [XmlEnum("50")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item50,

    /// <remarks />
    [XmlEnum("52")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item52,

    /// <remarks />
    [XmlEnum("53")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item53,

    /// <remarks />
    [XmlEnum("55")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item55,

    /// <remarks />
    [XmlEnum("56")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item56,

    /// <remarks />
    [XmlEnum("57")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item57,

    /// <remarks />
    [XmlEnum("58")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item58,

    /// <remarks />
    [XmlEnum("59")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item59,

    /// <remarks />
    [XmlEnum("60")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item60,

    /// <remarks />
    [XmlEnum("61")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item61,

    /// <remarks />
    [XmlEnum("62")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item62,

    /// <remarks />
    [XmlEnum("63")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item63,
}
