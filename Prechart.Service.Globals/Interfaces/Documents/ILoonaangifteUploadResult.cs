namespace Prechart.Service.Globals.Interfaces.Documents;

public interface ILoonaangifteUploadResult
{
    public string FileName { get; set; }
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; }
}