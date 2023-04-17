using Prechart.Service.Globals.Models.Loonheffings.Enums;
using Prechart.Service.Globals.Models.Person;

namespace Prechart.Service.Globals.Interfaces.Person;

public interface IPerson
{
    public string SofiNr { get; set; }
    public string Voorletter { get; set; }
    public string Voorvoegsel { get; set; }
    public string SignificantAchternaam { get; set; }
    public System.DateTime Geboortedatum { get; set; }
    public string Nationaliteit { get; set; }
    public Gesl Geslacht { get; set; }
    public List<ContactInfoModel> ContactInfos { get; set; }
    public List<PersonAddressesModel> PersonAddresses { get; set; }
}