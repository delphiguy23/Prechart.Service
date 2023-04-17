using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Prechart.Service.Core.Utils;
using Prechart.Service.Globals.Interfaces.Klant;
using Prechart.Service.Globals.Models.Person;

namespace Prechart.Service.Werkgever.Models;

[BsonIgnoreExtraElements]
public class KlantModel : IKlant
{
    [JsonConverter(typeof(StringToObjectId))]
    [BsonId]
    public ObjectId Id { get; set; }

    public string KlantNaam { get; set; }
    public List<string> Werkgevers { get; set; }
    public List<PersonModel> ContactPersons { get; set; }
    public bool Active { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateLastModified { get; set; }
}
