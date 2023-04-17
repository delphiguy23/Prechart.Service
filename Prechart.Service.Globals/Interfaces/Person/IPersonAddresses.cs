using Prechart.Service.Globals.Interfaces.Loonheffings;
using Prechart.Service.Globals.Models.Person;

namespace Prechart.Service.Globals.Interfaces.Person;

public interface IPersonAddresses
{
    public AddressBinnenlandModel AdresBinnenland { get; set; }
    public AddressBuitenlandModel AdresBuitenland { get; set; }
    public string AdresDescription { get; set; }
}