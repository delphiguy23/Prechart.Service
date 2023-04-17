using System.ComponentModel.DataAnnotations;

namespace Prechart.Service.Belastingen.Models.Berekeningen;

public enum HighLowEnum
{
    [Display(Name = "Laag")]
    Laag = 0,
    
    [Display(Name = "Hoog")]
    Hoog,
}