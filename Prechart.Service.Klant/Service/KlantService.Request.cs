using Prechart.Service.Globals.Models.Klant;

namespace Prechart.Service.Klant.Service;

public partial class KlantService
{
    public record UpsertKlants
    {
        public List<KlantModel> Klants { get; set; }
    }

    public record GetKlants;

    public record GetKlant
    {
        public string KlantId { get; set; }
        public string KlantName { get; set; }
        public string TaxNo { get; set; }
    }
}