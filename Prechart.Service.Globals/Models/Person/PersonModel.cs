using Prechart.Service.Globals.Interfaces.Person;
using Prechart.Service.Globals.Models.Loonheffings.Enums;

namespace Prechart.Service.Globals.Models.Person;

public class PersonModel : IPerson
{
    public string SofiNr { get; set; }
    public string Voorletter { get; set; }
    public string Voorvoegsel { get; set; }
    public string SignificantAchternaam { get; set; }
    public DateTime Geboortedatum { get; set; }
    public string Nationaliteit { get; set; }
    public Gesl Geslacht { get; set; }
    public List<ContactInfoModel> ContactInfos { get; set; }
    public List<PersonAddressesModel> PersonAddresses { get; set; }
}