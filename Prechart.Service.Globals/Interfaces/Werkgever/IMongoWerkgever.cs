using MongoDB.Bson;
using Newtonsoft.Json;
using Prechart.Service.Core.Utils;
using Prechart.Service.Globals.Models.Loonheffings;
using Prechart.Service.Globals.Models.Werkgever;

namespace Prechart.Service.Globals.Interfaces.Werkgever;

public interface IMongoWerkgever
{
    [JsonConverter(typeof(StringToObjectId))]
    ObjectId Id { get; set; }

    WerkgeverKlantModel Klant { get; set; }
    string Naam { get; set; }
    int Sector { get; set; }
    string FiscaalNummer { get; set; }
    string LoonheffingenExtentie { get; set; }
    string OmzetbelastingExtentie { get; set; }
    List<MongoWhkPremie> WhkPremies { get; set; }
    DateTime DatumActiefVanaf { get; set; }
    DateTime DatumActiefTot { get; set; }
    bool Actief { get; set; }
    DateTime DateCreated { get; set; }
    DateTime DateLastModified { get; set; }
    public List<CollectieveAangifteModel> Collectieve { get; set; }
}
