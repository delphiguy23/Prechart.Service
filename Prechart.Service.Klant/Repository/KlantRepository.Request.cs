using Prechart.Service.Globals.Models.Klant;

namespace Prechart.Service.Klant.Repository;

public partial class KlantRepository
{
    public record UpsertKlants
    {
        public List<KlantModel> Klants { get; set; }
    }

    public record GetKlants;

    public record GetKlantById
    {
        public string KlantId { get; set; }
    }
    
    public record GetKlantByName
    {
        public string KlantName { get; set; }
    }

    public record GetKlantByTaxNo
    {
        public string TaxNo { get; set; }
    }
}