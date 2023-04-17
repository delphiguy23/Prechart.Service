using Prechart.Service.Globals.Interfaces.Person;
using Prechart.Service.Globals.Models.Person;

namespace Prechart.Service.Klant.Models;

public class ContactInfoModel : IContactInfo
{
    public PersonContactType ContactType { get; set; }
    public string Contact { get; set; }
    public string ContactDescription { get; set; }
}