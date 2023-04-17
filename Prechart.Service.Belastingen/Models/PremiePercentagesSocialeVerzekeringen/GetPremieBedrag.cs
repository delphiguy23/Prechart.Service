using System;

namespace Prechart.Service.Belastingen.Models.PremiePercentagesSocialeVerzekeringen;

public class GetPremieBedrag
{
    public decimal LoonSocialVerzekeringen { get; set; }
    public decimal LoonZiektekostenVerzekeringsWet { get; set; }
    public DateTime SocialeVerzekeringenDatum { get; set; }
}