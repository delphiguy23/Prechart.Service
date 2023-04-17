namespace Prechart.Service.AuditLog.Interfaces.PremieBedrag;

public interface IGetPremieBedrag
{
    public decimal LoonSocialVerzekeringen { get; set; }
    public decimal LoonZiektekostenVerzekeringsWet { get; set; }
    public DateTime SocialeVerzekeringenDatum { get; set; }
}