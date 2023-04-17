using System;
using Prechart.Service.Globals.Models.Belastingen;
using Prechart.Service.Globals.Interfaces.PremieBedrag;
using Prechart.Service.Globals.Interfaces.Werkgever;

namespace Prechart.Service.Belastingen.Models.Berekeningen;

public class BerekeningenModel
{
    public int Id { get; set; }
    public DateTime ProcessDatum { get; set; }
    public int WoonlandbeginselId { get; set; }
    public int TijdvakId { get; set; }
    public decimal InkomenWit { get; set; }
    public decimal InkomenGroen { get; set; }
    public IMongoWerkgever Werkgever { get; set; }    
    public IBerekenenInhouding Inhouding { get; set; }    
    public  IPremieBedrag PremieBedrag { get; set; }
    public string EmployeeId { get; set; }
    public DateTime LoonOverVanaf { get; set; }
    public DateTime LoonOverTot { get; set; }
    public DateTime LoonInVanaf { get; set; }
    public DateTime LoonInTot { get; set; }
    public int AlgemeneHeffingskortingToegepast { get; set; }
    public decimal BasisDagen { get; set; }
    public decimal InhoudingOpLoonWit { get; set; }
    public decimal InhoudingOpLoonGroen { get; set; }
    public decimal AlgemeneHeffingskortingBedrag { get; set; }
    public decimal VerrekendeArbeIdskorting { get; set; }
    public decimal SociaalVerzekeringsloon { get; set; }
    
    public HighLowEnum PremieBedragAlgemeenWerkloosheIdsFondsLaagHoog { get; set; }
    public decimal PremieBedragAlgemeenWerkloosheIdsFondsLaag { get; set; }
    public decimal PremieBedragAlgemeenWerkloosheIdsFondsHoog { get; set; }
    
    public bool IsPremieBedragUitvoeringsFondsvoordeOverheId { get; set; }
    public decimal PremieBedragUitvoeringsFondsvoordeOverheId { get; set; }
    public HighLowEnum PremieBedragWetArbeIdsOngeschikheIdLaagHoog { get; set; }
    public decimal PremieBedragWetArbeIdsOngeschikheIdLaag { get; set; }
    public decimal PremieBedragWetArbeIdsOngeschikheIdHoog { get; set; }
    
    public decimal PremieBedragWetKinderopvang { get; set; }
    public decimal PremieBedragZiektekostenVerzekeringsWetLoon { get; set; }
    
    public PayeeEnum Payee { get; set; }
    public decimal PremieBedragZiektekostenVerzekeringsWetWerkgeversbijdrage { get; set; }
    public decimal PremieBedragZiektekostenVerzekeringsWetWerknemersbijdrage { get; set; }
    public decimal WerkgeverWHKPremieBedragWGAVastWerkgever { get; set; }
    public decimal WerkgeverWHKPremieBedragWGAVastWerknemer { get; set; }
    public decimal WerkgeverWHKPremieBedragFlexWerkgever { get; set; }
    public decimal WerkgeverWHKPremieBedragFlexWerknemer { get; set; }
    public decimal WerkgeverWHKPremieBedragZWFlex { get; set; }
    public decimal WerkgeverWHKPremieBedragTotaal { get; set; }
    public decimal NettoTeBetalenSubTotaal { get; set; }
    public decimal NettoTeBetalenEindTotaal { get; set; }
    public bool Deleted { get; set; }
    public bool Actief { get; set; }
}