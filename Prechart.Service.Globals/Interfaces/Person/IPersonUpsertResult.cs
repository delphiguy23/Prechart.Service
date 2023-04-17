using Prechart.Service.Globals.Models;

namespace Prechart.Service.Globals.Interfaces.Person;

public interface IPersonUpsertResult
{
    public string Id { get; set; }
    public string Bsn { get; set; }
    public string TaxFileName { get; set; }
    public DataOperation Operation { get; set; }
}