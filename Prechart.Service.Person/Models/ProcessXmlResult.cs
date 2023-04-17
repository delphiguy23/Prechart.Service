using Prechart.Service.Globals.Interfaces.Loonheffings;

namespace Prechart.Service.Person.Models;

public class ProcessXmlResult : ILoonaangifteProcessResult
{
    public string FileName { get; set; }
    public bool Processed { get; set; }
    public string ProcessErrors { get; set; }
    public int EmployeesInserted { get; set; }
    public int EmployeesUpdated { get; set; }
}
