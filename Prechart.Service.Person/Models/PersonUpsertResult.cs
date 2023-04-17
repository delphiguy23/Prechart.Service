using Prechart.Service.Globals.Interfaces.Person;
using Prechart.Service.Globals.Models;

namespace Prechart.Service.Person.Models;

public class PersonUpsertResult : IPersonUpsertResult
{
    public string Id { get; set; }
    public string Bsn { get; set; }
    public string TaxFileName { get; set; }
    public DataOperation Operation { get; set; }
}