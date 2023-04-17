using System.Collections.Generic;
using Prechart.Service.Globals.Interfaces.Documents;

namespace Prechart.Service.Documents.Upload.Models;

public class LoonaangifteUploadEvent : ILoonaangifteUploadEvent
{
    public List<IXmlStream> Files { get; set; }
    public int XsdYear { get; set; }
}