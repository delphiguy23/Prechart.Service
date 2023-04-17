using Prechart.Service.Globals.Models.Person;

namespace Prechart.Service.Globals.Interfaces.Person;

public interface IUpdatePersonPhoto
{
    public string Id { get; set; }
    public PersonPhotoModel PersonPhoto { get; set; }
}
