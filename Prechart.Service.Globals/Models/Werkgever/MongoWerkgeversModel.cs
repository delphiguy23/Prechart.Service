using Prechart.Service.Globals.Interfaces.Werkgever;

namespace Prechart.Service.Globals.Models.Werkgever;

public class MongoWerkgeversModel : IMongoWerkgevers
{
    public List<IMongoWerkgever> Werkgevers { get; set; }
}