using System.ComponentModel.DataAnnotations;

namespace Prechart.Service.Belastingen.Models.Berekeningen;

public enum PayeeEnum
{
    None = 0,
    [Display(Name = "Werkgever")]
    Werkgever,

    [Display(Name = "Werknemer")]
    Werknemer,
}