using Prechart.Service.Globals.Models.Loonheffings.Enums;
using Prechart.Service.Globals.Models.Person;
using Prechart.Service.Globals.Models.Xsd.Loonheffings;
using System;

namespace Prechart.Service.Person.Models;

public class PersonUserModel
{
    public string Id { get; set; }
    public string SofiNr { get; set; }
    public string Voorletter { get; set; }
    public string Voorvoegsel { get; set; }
    public string SignificantAchternaam { get; set; }
    public DateTime Geboortedatum { get; set; }
    public string Nationaliteit { get; set; }
    public Gesl Geslacht { get; set; }
    public AdresBinnenlandType AdresBinnenland { get; set; }
    public AdresBuitenlandType AdresBuitenland { get; set; }
    public PersonType PersonType { get; set; }
    public bool Active { get; set; }
}
