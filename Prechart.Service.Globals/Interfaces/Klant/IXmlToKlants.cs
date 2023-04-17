using Prechart.Service.Globals.Models.Klant;

namespace Prechart.Service.Globals.Interfaces.Klant;

public interface IXmlToKlants 
{
    public List<KlantModel> Klants { get; set; }
}