using System.Text.Json.Serialization;
using System.Xml.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Prechart.Service.Globals.Models.Loonheffings.Enums;

public enum CdAard
{
    /// <remarks />
    [XmlEnum("1")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item1,

    /// <remarks />
    [XmlEnum("4")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item4,

    /// <remarks />
    [XmlEnum("6")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item6,

    /// <remarks />
    [XmlEnum("7")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item7,

    /// <remarks />
    [XmlEnum("11")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item11,

    /// <remarks />
    [XmlEnum("18")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item18,

    /// <remarks />
    [XmlEnum("21")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item21,

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
    [XmlEnum("79")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item79,

    /// <remarks />
    [XmlEnum("81")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item81,

    /// <remarks />
    [XmlEnum("82")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item82,

    /// <remarks />
    [XmlEnum("83")]
    [JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json.Serialization
    [BsonRepresentation(BsonType.String)]
    Item83,
}
