using System;

namespace Prechart.Service.Person.Services.Daywage;

public partial class DaywageService
{
    public record GetReferencePeriod
    {
        public DateTime StartOfSickLeave { get; set; }
    }

    public record CalculateWithInReferencePeriod
    {
        public string PersonId { get; set; }
        public string TaxNumber { get; set; }
        public DateTime StartOfSickLeave { get; set; }
    }

    public record GetDaywage
    {
        public string PersonId { get; set; }
        public string TaxNumber { get; set; }
    }
}
