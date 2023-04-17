using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Prechart.Service.Person.Models;

public class PersonCredentialsModel
{
    [BsonId] public ObjectId Id { get; set; }

    public string SofiNr { get; set; }
    public string SofiLast4 { get; set; }
    public string PersNr { get; set; }
    public DateTime Geboortedatum { get; set; }
}
