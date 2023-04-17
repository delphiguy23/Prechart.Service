using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Prechart.Service.Core.Utils;
using Prechart.Service.Globals.Interfaces.Werkgever;

namespace Prechart.Service.Globals.Models.Werkgever;

[BsonIgnoreExtraElements]
public class MongoWhkPremie: IMongoWhkPremie
{
    [BsonId]
    [JsonConverter(typeof(StringToObjectId))]
    public ObjectId Id { get; set; }
    
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal WgaVastWerkgever { get; set; }
    
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal WgaVastWerknemer { get; set; }
    
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal FlexWerkgever { get; set; }
    
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal FlexWerknemer { get; set; }
    
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal ZwFlex { get; set; }
    
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Totaal => WgaVastWerkgever + WgaVastWerknemer + FlexWerkgever + FlexWerknemer + ZwFlex;

    public DateTime ActiefVanaf { get; set; }
    public DateTime ActiefTot { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateLastModified { get; set; }
    public int? SqlId { get; set; }

    public bool Actief { get; set; }
}