namespace Prechart.Service.Globals.Interfaces.PremieBedrag;

public interface IPremieBedrag
{
    public decimal PremieBedragAlgemeneOuderdomsWet { get; set; }
    public decimal PremieBedragNabestaanden { get; set; }
    public decimal PremieBedragWetLangdurigeZorg { get; set; }
    public decimal PremieBedragSocialeVerzekeringenPremieloon { get; set; }
    public decimal PremieBedragAlgemeenWerkloosheidsFondsLaag { get; set; }
    public decimal PremieBedragAlgemeenWerkloosheidsFondsHoog { get; set; }
    public decimal PremieBedragUitvoeringsFondsvoordeOverheid { get; set; }
    public decimal PremieBedragWetArbeidsOngeschikheidLaag { get; set; }
    public decimal PremieBedragWetArbeidsOngeschikheidHoog { get; set; }
    public decimal PremieBedragWetKinderopvang { get; set; }
    public decimal PremieBedragZiektekostenVerzekeringsWetWerkgeversbijdrage { get; set; }
    public decimal PremieBedragZiektekostenVerzekeringsWetWerknemersbijdrage { get; set; }
    public decimal PremieBedragZiektekostenVerzekeringsWetLoon { get; set; }
}