using System.Text.Json.Serialization;
using System.Xml.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Prechart.Service.Globals.Models.Loonheffings.Enums;

public enum FsIndFZ
{
    /// <remarks />
    [XmlEnum("1")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item1,

    /// <remarks />
    [XmlEnum("3")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item3,

    /// <remarks />
    [XmlEnum("4")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item4,

    /// <remarks />
    [XmlEnum("5")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item5,

    /// <remarks />
    [XmlEnum("6")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item6,

    /// <remarks />
    [XmlEnum("17")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item17,

    /// <remarks />
    [XmlEnum("18")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item18,

    /// <remarks />
    [XmlEnum("19")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item19,

    /// <remarks />
    [XmlEnum("38")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item38,

    /// <remarks />
    [XmlEnum("40")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item40,

    /// <remarks />
    [XmlEnum("41")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item41,

    /// <remarks />
    [XmlEnum("43")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item43,
}
