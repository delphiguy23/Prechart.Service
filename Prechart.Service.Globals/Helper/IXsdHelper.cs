namespace Prechart.Service.Globals.Helper;

public interface IXsdHelper
{
    Stream GetSchemaStream(string xsdFileName);
    Task<List<string>> Validate(byte[] xmlStream, int xsdYear);
}
