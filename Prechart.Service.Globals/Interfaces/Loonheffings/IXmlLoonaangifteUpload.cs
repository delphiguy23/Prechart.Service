using MongoDB.Bson;

namespace Prechart.Service.Globals.Interfaces.Loonheffings;

public interface IXmlLoonaangifteUpload
{
    ObjectId Id { get; set; }
    string FileName { get; set; }
    bool IsValid { get; set; }
    List<string> Errors { get; set; }
    object Loonaangifte { get; set; }
    int EmployeesInserted { get; set; }
    int EmployeesUpdated { get; set; }
    bool Processed { get; set; }
    DateTime UploadedDate { get; set; }
    DateTime ProcessedDate { get; set; }
}