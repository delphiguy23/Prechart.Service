using Prechart.Service.Globals.Interfaces.Person;
using Prechart.Service.Globals.Models.Documents;

namespace Prechart.Service.Person.Models;

public class PersonDownloadModel : IDownloadPersonPhotoResults
{
    ////public FileContentResult FileDownload { get; set; }
    public string Filename { get; set; }
    public byte[] FileData { get; set; }
    public MediaType MediaType { get; set; }
    public MediaSubtype MediaSubtype { get; set; }
}
