namespace Prechart.Service.Werkgever.Database.Models;

public class WerkgeverWhkPremies
{
    public int Id { get; set; }
    public int WerkgeverId { get; set; }
    public string WerkgeverWhkMongoId { get; set; }
    public decimal WgaVastWerkgever { get; set; }
    public decimal WgaVastWerknemer { get; set; }
    public decimal FlexWerkgever { get; set; }
    public decimal FlexWerknemer { get; set; }
    public decimal ZwFlex { get; set; }
    public decimal Totaal { get; set; }
    public DateTime ActiefVanaf { get; set; }
    public DateTime ActiefTot { get; set; }
    public bool Actief { get; set; }
}