using Prechart.Service.Globals.Interfaces.Werkgever;

namespace Prechart.Service.Werkgever.Models;

public class KlantWerkgeverModel : IWerkgeverKlant
{
    public string KlantId { get; set; }
    public string KlantName { get; set; }
}