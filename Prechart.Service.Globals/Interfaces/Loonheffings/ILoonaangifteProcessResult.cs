namespace Prechart.Service.Globals.Interfaces.Loonheffings;

public interface ILoonaangifteProcessResult
{
    string FileName { get; set; }
    bool Processed { get; set; }
    string ProcessErrors { get; set; }
    int EmployeesInserted { get; set; }
    int EmployeesUpdated { get; set; }
}
