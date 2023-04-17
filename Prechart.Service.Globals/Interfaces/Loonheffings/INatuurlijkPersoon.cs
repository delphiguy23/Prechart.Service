using Prechart.Service.Globals.Models.Xsd.Loonheffings;

namespace Prechart.Service.Globals.Interfaces.Loonheffings;

public interface INatuurlijkPersoon
{
    IXmlToPerson Person { get; set; }
    WerknemersgegevensType Werknemersgegevens { get; set; }
}