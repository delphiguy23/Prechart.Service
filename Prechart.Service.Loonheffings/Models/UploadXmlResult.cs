using System.Collections.Generic;
using Prechart.Service.Globals.Interfaces.Documents;

namespace Prechart.Service.Loonheffings.Models;

public class UploadXmlResult : ILoonaangifteUploadResult
{
    public string FileName { get; set; }
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; }
    public int EmployeesInserted { get; set; }
    public int EmployeesUpdated { get; set; }
}