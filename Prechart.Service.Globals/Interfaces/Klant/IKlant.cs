using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Prechart.Service.Globals.Interfaces.Person;
using Prechart.Service.Globals.Models.Person;

namespace Prechart.Service.Globals.Interfaces.Klant;

public interface IKlant
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string KlantNaam { get; set; }
    public List<string> Werkgevers { get; set; }
    public List<PersonModel> ContactPersons { get; set; }
    public bool Active { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateLastModified { get; set; }
}