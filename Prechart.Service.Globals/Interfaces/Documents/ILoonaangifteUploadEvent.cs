namespace Prechart.Service.Globals.Interfaces.Documents;


public interface ILoonaangifteUploadEvent
{
    public List<IXmlStream> Files { get; set; }
    public int XsdYear { get; set; }
}