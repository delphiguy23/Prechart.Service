using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Prechart.Service.Globals.Interfaces.Documents;

namespace Prechart.Service.Loonheffings.Service;

public partial class LoonheffingsService
{
    public record ValidateXml
    {
        public List<IXmlStream> Files { get; set; }
        public int XsdYear { get; set; }
    }

    public record ExtractEmployeeFromXml
    {
        public string FileName { get; set; }
        public IFormFile FileContent { get; set; }
    }

    public record ProcessUploadedXmls;
    
    public record LoonheffingsProcessedResult
    {
        public string FileName { get; set; }
        public bool Processed { get; set; }
        public string ProcessErrors { get; set; }
        public int EmployeesInserted { get; set; }
        public int EmployeesUpdated { get; set; }
    }
}
