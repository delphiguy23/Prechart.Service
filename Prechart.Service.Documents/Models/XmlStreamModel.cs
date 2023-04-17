using Prechart.Service.Globals.Interfaces.Documents;

namespace Prechart.Service.Documents.Upload.Models;

public class XmlStreamModel : IXmlStream
{
    public string FileName { get; set; }
    public byte[] Stream { get; set; }
}