using System.ComponentModel.DataAnnotations;

namespace Prechart.Service.Belastingen.Database.Models;

public class Landen 
{
    [Key] public int Id { get; set; }
    [Required] public string LandenCode { get; set; }
    [Required] public string LandenNaam { get; set; }
    [Required] public int WoonlandbeginselIndicatie { get; set; }

    public bool Active { get; set; }
}