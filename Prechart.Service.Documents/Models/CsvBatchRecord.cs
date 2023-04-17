namespace Prechart.Service.Documents.Upload.Models;

public record CsvBatchRecord
{
    public string FileName { get; set; }
    public byte[] CsvFile { get; set; }
}
