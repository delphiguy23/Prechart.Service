using System.Collections.Generic;
using Prechart.Service.Loonheffings.Models;

namespace Prechart.Service.Loonheffings.Repositories;

public partial class LoonheffingsRepository
{
    public record UpsertTaxFiling
    {
        public TaxFiling TaxFiling { get; set; }
    }

    public record UpsertEmployeesOnBoarding
    {
        public List<EmployeeOnBoardingModel> EmployeesOnBoarding { get; set; }
    }

    public record UpsertTaxFiling2022
    {
        public XmlLoonaangifteUpload TaxFiling { get; set; }
    }

    public record GetUnProcessUploadedTaxFiling;

    public record GetUnProcessUploadedTaxFilingWithoutEmployees;

    public record UpdateProcessedXml
    {
        public string FileName { get; set; }
        public int EmployeesInserted { get; set; }
        public int EmployeesUpdated { get; set; }
        public string Errors { get; set; }
    }

    public record LoonheffingsProcessedResult
    {
        public string FileName { get; set; }
        public bool Processed { get; set; }
        public string ProcessErrors { get; set; }
        public int EmployeesInserted { get; set; }
        public int EmployeesUpdated { get; set; }
    }
}
