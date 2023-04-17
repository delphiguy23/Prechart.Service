namespace Prechart.Service.Werkgever.Database.Models;

public class Werkgever
{
    public int Id { get; set; }
    public string WerkgeverMongoId { get; set; }
    public string KlantMongoId { get; set; }
    public string Naam { get; set; }
    public int Sector { get; set; }
    public string FiscaalNummer {get;set; }
    public string LoonheffingenExtentie { get; set; }
    public string OmzetbelastingExtentie { get; set; }
    public DateTime DatumActiefVanaf { get; set; }
    public DateTime DatumActiefTot { get; set; }
    public bool Actief { get; set; }
}