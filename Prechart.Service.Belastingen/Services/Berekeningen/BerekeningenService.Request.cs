using Prechart.Service.Belastingen.Models.Berekeningen;

namespace Prechart.Service.Belastingen.Services.Berekeningen;

public partial class BerekeningenService
{
    public record CalculateBerekenen
    {
        public GetBerekenen Parameters { get; set; }
    }
}