using Prechart.Service.Globals.Interfaces.Loonheffings;
using Prechart.Service.Globals.Interfaces.Person;

namespace Prechart.Service.Globals.Models.Person;

public class PersonAddressesModel : IPersonAddresses
{
    public AddressBinnenlandModel AdresBinnenland { get; set; }
    public AddressBuitenlandModel AdresBuitenland { get; set; }
    public string AdresDescription { get; set; }
}