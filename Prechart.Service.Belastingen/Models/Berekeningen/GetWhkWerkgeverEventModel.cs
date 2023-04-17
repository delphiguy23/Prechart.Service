using Prechart.Service.Globals.Interfaces.Werkgever;

namespace Prechart.Service.Belastingen.Models.Berekeningen;

public class GetWhkWerkgeverEventModel : IMongoGetWerkgever
{
    public string Taxno { get; set; }
}