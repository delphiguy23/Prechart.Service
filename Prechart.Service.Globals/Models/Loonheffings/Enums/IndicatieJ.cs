using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Prechart.Service.Globals.Models.Loonheffings.Enums;

public enum IndicatieJ
{
    /// <remarks />
    [JsonConverter(typeof(JsonStringEnumConverter))] [BsonRepresentation(BsonType.String)]
    J,
}
