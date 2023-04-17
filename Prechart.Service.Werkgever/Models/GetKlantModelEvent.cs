using Prechart.Service.Globals.Interfaces.Klant;

namespace Prechart.Service.Werkgever.Models;

public class GetKlantModelEvent : IGetKlant
{
    public string KlantId { get; set; }
    public string KlantName { get; set; }
    public string TaxNo { get; set; }
}