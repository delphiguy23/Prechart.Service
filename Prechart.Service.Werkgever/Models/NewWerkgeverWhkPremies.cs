namespace Prechart.Service.Werkgever.Models;

public class NewWerkgeverWhkPremies
{
    public int WerkgeverId { get; set; }
    public decimal WgaVastWerkgever { get; set; }
    public decimal WgaVastWerknemer { get; set; }
    public decimal FlexWerkgever { get; set; }
    public decimal FlexWerknemer { get; set; }
    public decimal ZwFlex { get; set; }
    public decimal WgaFlexWerknemer { get; set; }
    public decimal Totaal { get; set; }
    public DateTime ActiefVanaf { get; set; }
    public DateTime ActiefTot { get; set; }
    public bool Actief { get; set; }  
}