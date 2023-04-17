using System;
using Prechart.Service.AuditLog.Interfaces.PremieBedrag;

namespace Prechart.Service.Belastingen.Models.PremiePercentagesSocialeVerzekeringen;

public class GetPremieBedragEventModel : IGetPremieBedrag
{
    public decimal LoonSocialVerzekeringen { get; set; }
    public decimal LoonZiektekostenVerzekeringsWet { get; set; }
    public DateTime SocialeVerzekeringenDatum { get; set; }
}