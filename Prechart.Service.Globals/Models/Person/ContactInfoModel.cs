using Prechart.Service.Globals.Interfaces.Person;

namespace Prechart.Service.Globals.Models.Person;

public class ContactInfoModel : IContactInfo
{
    public PersonContactType ContactType { get; set; }
    public string Contact { get; set; }
    public string ContactDescription { get; set; }
}