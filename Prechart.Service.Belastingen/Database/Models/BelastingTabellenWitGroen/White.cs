using System.ComponentModel.DataAnnotations;

namespace Prechart.Service.Belastingen.Database.Models;

public class White
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int Year { get; set; }

    [Required]
    public int CountryId { get; set; }

    [Required]
    public int TypeId { get; set; }

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
