using Prechart.Service.Globals.Interfaces.Belastingen;

namespace Prechart.Service.Belastingen.Models;

public class GetWoonlandbeginsel : IGetWoonlandbeginselEvent
{
    public string WoonlandbeginselCode { get; set; }
}

