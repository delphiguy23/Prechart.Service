using Prechart.Service.Globals.Models.Documents;

namespace Prechart.Service.Globals.Interfaces.Person;

public interface IDownloadPersonPhotoResults
{
    public string Filename { get; set; }
    public byte[] FileData { get; set; }
    public MediaType MediaType { get; set; }
    public MediaSubtype MediaSubtype { get; set; }
}
