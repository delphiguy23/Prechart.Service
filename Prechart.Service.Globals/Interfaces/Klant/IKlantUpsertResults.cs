using Prechart.Service.Globals.Interfaces.Werkgever;

namespace Prechart.Service.Globals.Interfaces.Klant;

public interface IKlantUpsertResults
{
    bool isOk { get; set; }
    List<IUpsertWerkgever> Werkgevers { get; set; }
}