using System;
using Prechart.Service.AuditLog.Interfaces.PremieBedrag;

namespace Prechart.Service.Belastingen.Services.PremiePercentagesSocialeVerzekeringen;

public partial class PremiePercentagesSocialeVerzekeringenService
{
    public record GetPremiePercentage : IGetPremieBedrag
    {
        public decimal LoonSocialVerzekeringen { get; set; }
        public decimal LoonZiektekostenVerzekeringsWet { get; set; }
        public DateTime SocialeVerzekeringenDatum { get; set; }
    }
}