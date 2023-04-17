namespace Prechart.Service.Globals.Interfaces.Documents;

public interface IXmlStream
{
    public string FileName { get; set; }
    public byte[] Stream { get; set; }
}