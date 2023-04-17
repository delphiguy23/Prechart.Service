using MongoDB.Bson;

namespace Prechart.Service.Globals.Interfaces.Werkgever;

public interface IMongoWhkPremie
{
    ObjectId Id { get; set; }
    decimal WgaVastWerkgever { get; set; }
    decimal WgaVastWerknemer { get; set; }
    decimal FlexWerkgever { get; set; }
    decimal FlexWerknemer { get; set; }
    decimal ZwFlex { get; set; }
    decimal Totaal { get;  }
    DateTime ActiefVanaf { get; set; }
    DateTime ActiefTot { get; set; }
    DateTime DateCreated { get; set; }
    DateTime DateLastModified { get; set; }
    int? SqlId { get; set; }
    bool Actief { get; set; }
}