
using System.ComponentModel;

namespace Prechart.Service.Globals.Models.Belastingen;

public enum TaxPeriodEnum
{
    [Description("Geen")]
    None = 0,

    [Description("Dag")]
    Day = 1,

    [Description("Week")]
    Week = 2,

    [Description("Vier Wekelijks")]
    FourWeekly = 3,

    [Description("Maand")]
    Month = 4,

    [Description("Kwartaal")]
    Quarter = 5
}