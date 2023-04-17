using Prechart.Service.Globals.Interfaces.Loonheffings;
using Prechart.Service.Globals.Interfaces.Person;
using Prechart.Service.Globals.Models.Person;

namespace Prechart.Service.Klant.Models;

public class PersonAddressModel : IPersonAddresses
{
    public AddressBinnenlandModel AdresBinnenland { get; set; }
    public AddressBuitenlandModel AdresBuitenland { get; set; }
    public string AdresDescription { get; set; }
}