using Prechart.Service.Globals.Interfaces.Loonheffings;

namespace Prechart.Service.Globals.Models.Klant;

public class KlantWerkgeverModel : IXmlWerkgever
{
    public string Klant { get; set; }
    public string LoonheffingsNr { get; set; }
}