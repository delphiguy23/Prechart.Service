using Prechart.Service.Globals.Interfaces.Werkgever;

namespace Prechart.Service.Globals.Models.Werkgever;

public class UpsertWerkgeverModel : IUpsertWerkgever
{
    public string KlantId { get; set; }
    public string KlantName { get; set; }
    public string TaxNo { get; set; }
}