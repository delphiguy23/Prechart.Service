using Prechart.Service.Globals.Interfaces.Belastingen;

namespace Prechart.Service.Belastingen.Database.Models;

public class InsertToTaxTable : IInsertToTaxTable
{
    public bool IsProcessed { get; set; } = false;
}

