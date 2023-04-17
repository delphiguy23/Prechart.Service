using Prechart.Service.Globals.Interfaces.Werkgever;

namespace Prechart.Service.Globals.Models.Werkgever;

public class WerkgeverKlantModel : IWerkgeverKlant
{
    public string KlantId { get; set; }
    public string KlantName { get; set; }
}