using Prechart.Service.Person.Models.Daywage;

namespace Prechart.Service.Person.Repositories.Daywage;

public partial class DaywageRepository
{
    public record GetTaxDetails
    {
        public string PersonId { get; set; }
        public string TaxNumber { get; set; }
    }

    public record UpsertDaywage
    {
        public DaywageWithInReferencePeriode DaywageWithInReferencePeriode { get; set; }
    }

    public record GetDaywage
    {
        public string PersonId { get; set; }
        public string TaxNumber { get; set; }
    }
}
