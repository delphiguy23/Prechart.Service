using Prechart.Service.Globals.Interfaces.Loonheffings;

namespace Prechart.Service.Globals.Models.Person;

public class AddressBuitenlandModel : IXmlAdresBuitenland
{
    public string Str { get; set; }
    public string HuisNr { get; set; }
    public string LocOms { get; set; }
    public string Pc { get; set; }
    public string Woonpl { get; set; }
    public string Reg { get; set; }
    public string LandCd { get; set; }
}