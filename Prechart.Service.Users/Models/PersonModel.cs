using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Prechart.Service.Globals.Interfaces.Loonheffings;
using Prechart.Service.Globals.Models.Klant;
using Prechart.Service.Globals.Models.Loonheffings;
using Prechart.Service.Globals.Models.Loonheffings.Enums;
using Prechart.Service.Globals.Models.Person;
using Prechart.Service.Globals.Models.Person.Daywages;
using Prechart.Service.Globals.Models.Xsd.Loonheffings;

namespace Prechart.Service.Users.Models;

[BsonIgnoreExtraElements]
public class PersonModel : IXmlToPerson
{
    [BsonId] public ObjectId Id { get; set; }

    public string SofiNr { get; set; }
    public string Voorletter { get; set; }
    public string Voorvoegsel { get; set; }
    public string SignificantAchternaam { get; set; }
    public DateTime Geboortedatum { get; set; }
    public string Nationaliteit { get; set; }
    public Gesl Geslacht { get; set; }
    public List<KlantWerkgeverModel> Werkgever { get; set; }
    public AdresBinnenlandType AdresBinnenland { get; set; }
    public AdresBuitenlandType AdresBuitenland { get; set; }
    public PersonType PersonType { get; set; }
    public bool Active { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateLastModified { get; set; }
    public List<TaxPaymentDetails> TaxPaymentDetails { get; set; }
    public string EmailAddress { get; set; }
    public string UserName { get; set; }
    [BsonIgnoreIfNull] public List<PersonDaywageModel> PersonDaywages { get; set; }
    [BsonIgnoreIfNull] public PersonPhotoModel PersonPhoto { get; set; }
    [BsonIgnore] public string TaxFileName { get; set; }
}
