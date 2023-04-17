using Prechart.Service.Globals.Models.Documents;

namespace Prechart.Service.Globals.Models.Person;

public class PersonPhotoModel
{
    public string Filename { get; set; }
    public byte[] FileData { get; set; }
    public MediaType MediaType { get; set; }
    public MediaSubtype MediaSubtype { get; set; }
}