using Prechart.Service.Globals.Interfaces.Werkgever;

namespace Prechart.Service.Belastingen.Models.Werkgever;

public class GetWerkgever : IMongoGetWerkgever
{
    public string Taxno { get; set; }
}