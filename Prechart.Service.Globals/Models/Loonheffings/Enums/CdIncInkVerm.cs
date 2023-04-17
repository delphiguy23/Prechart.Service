using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Prechart.Service.Globals.Models.Loonheffings.Enums;

public enum CdIncInkVerm
{
    /// <remarks />
    [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    K,

    /// <remarks />
    [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    O,

    /// <remarks />
    [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    S,

    /// <remarks />
    [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    Z,
}
