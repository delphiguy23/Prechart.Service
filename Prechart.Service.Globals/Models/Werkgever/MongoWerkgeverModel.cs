using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Prechart.Service.Core.Utils;
using Prechart.Service.Globals.Interfaces.Werkgever;
using Prechart.Service.Globals.Models.Loonheffings;

namespace Prechart.Service.Globals.Models.Werkgever;

[BsonIgnoreExtraElements]
public class MongoWerkgeverModel : IMongoWerkgever
{
    public List<CollectieveAangifteModel> Collectieve { get; set; }

    [BsonId]
    [JsonConverter(typeof(StringToObjectId))]
    public ObjectId Id { get; set; }

    public WerkgeverKlantModel Klant { get; set; }
    public string Naam { get; set; }
    public int Sector { get; set; }
    public string FiscaalNummer { get; set; }
    public string LoonheffingenExtentie { get; set; }
    public string OmzetbelastingExtentie { get; set; }
    public List<MongoWhkPremie> WhkPremies { get; set; }
    public DateTime DatumActiefVanaf { get; set; }
    public DateTime DatumActiefTot { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateLastModified { get; set; }
    public bool Actief { get; set; }
}
