using Prechart.Service.Belastingen.Models.Berekeningen;

namespace Prechart.Service.Belastingen.Repositories.Berekeningen;

public partial class BerekeningenRepository
{
    public record UpsertBerekeningen
    {
        public BerekeningenModel Berekeningen { get; set; }
    }
}