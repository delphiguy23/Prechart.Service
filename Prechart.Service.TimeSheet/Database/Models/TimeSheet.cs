using System;
using System.ComponentModel.DataAnnotations;

namespace Prechart.Service.TimeSheet.Database.Models;

public class TimeSheet
{
    [Key] public int Id { get; set; }

    [Required] public int PersonId { get; set; }

    [Required] public DateTime Date { get; set; }

    public DateTime TimeIn { get; set; }
    public DateTime TimeOut { get; set; }
    public DateTime LunchIn { get; set; }
    public DateTime LunchOut { get; set; }
}