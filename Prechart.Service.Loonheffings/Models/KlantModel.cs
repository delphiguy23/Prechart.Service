using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Prechart.Service.Globals.Interfaces.Person;
using Prechart.Service.Globals.Interfaces.Klant;
using Prechart.Service.Globals.Models.Person;

namespace Prechart.Service.Loonheffings.Models;

[BsonIgnoreExtraElements]
public class KlantModel : IKlant
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