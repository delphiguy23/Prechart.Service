using System.Text.Json.Serialization;
using System.Xml.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Prechart.Service.Globals.Models.Loonheffings.Enums;

public enum CdRdnGnBijt
{
    /// <remarks />
    [XmlEnum("1")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item1,

    /// <remarks />
    [XmlEnum("2")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item2,

    /// <remarks />
    [XmlEnum("3")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item3,

    /// <remarks />
    [XmlEnum("5")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item5,

    /// <remarks />
    [XmlEnum("7")] [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Item7,
}
