using Prechart.Service.Globals.Models.Person.Daywages;

namespace Prechart.Service.Person.Models.Daywage;

public class DaywageWithInReferencePeriode
{
    public string PersonId { get; set; }
    public PersonDaywageModel DaywageRecord { get; set; }
}
