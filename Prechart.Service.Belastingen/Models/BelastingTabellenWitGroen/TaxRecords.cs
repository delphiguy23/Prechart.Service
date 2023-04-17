using Prechart.Service.Globals.Interfaces.Belastingen;

namespace Prechart.Service.Belastingen.Models;   

public class TaxRecords
{
    public ITaxRecord Green { get; set; }
    public ITaxRecord White { get; set; }
}

