using Prechart.Service.Globals.Models.Person;

namespace Prechart.Service.Globals.Interfaces.Person;

public interface IContactInfo
{
    public PersonContactType ContactType { get; set; }
    public string Contact { get; set; }
    public string ContactDescription { get; set; }
}