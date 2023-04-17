namespace Prechart.Service.Globals.Interfaces.Person.Daywage;

public interface IDaywage
{
    public DateTime StartOfDaywage { get; set; }
    public DateTime EndOfDaywage { get; set; }
    public decimal Daywage { get; set; }
    public bool Active { get; set; }
}