using System;

namespace Prechart.Service.Belastingen.Repositories.PremiePercentagesSocialeVerzekeringen;

public partial class PremiePercentagesSocialeVerzekeringenRepository
{
    public record GetPremiePercentage
    {
        public decimal LoonSocialVerzekeringen { get; set; }
        public decimal LoonZiektekostenVerzekeringsWet { get; set; }
        public DateTime SocialeVerzekeringenDatum { get; set; }
    }
}