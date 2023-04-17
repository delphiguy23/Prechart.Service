using Prechart.Service.Globals.Interfaces.Belastingen;

namespace Prechart.Service.Globals.Models.Belastingen;

public class TaxTableModel : ITaxRecord
{
    public int Id { get; set; }
    public int Year { get; set; }
    public int CountryId { get; set; }
    public string CountryName { get; set; }
    public int TypeId { get; set; }
    public string TypeName { get; set; }
    public decimal Tabelloon { get; set; }
    public decimal ZonderLoonheffingskorting { get; set; }
    public decimal MetLoonheffingskorting { get; set; }
    public decimal VerrekendeArbeidskorting { get; set; }
    public decimal EerderZonderLoonheffingskorting { get; set; }
    public decimal EerderMetLoonheffingskorting { get; set; }
    public decimal EerderVerrekendeArbeidskorting { get; set; }
    public decimal LaterZonderLoonheffingskorting { get; set; }
    public decimal LaterMetLoonheffingskorting { get; set; }
    public decimal LaterVerrekendeArbeidskorting { get; set; }
    public bool Active { get; set; }
}
