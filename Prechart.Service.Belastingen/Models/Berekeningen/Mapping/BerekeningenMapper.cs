using AutoMapper;

namespace Prechart.Service.Belastingen.Models.Berekeningen.Mapping;

public class BerekeningenMapper : Profile
{
    public BerekeningenMapper()
    {
        CreateMap<BerekeningenModel, Database.Models.Berekeningen.Berekeningen>()
            .ForMember(d => d.ProcessDatum, o => o.MapFrom(s => s.ProcessDatum))
            .ForMember(d => d.WoonlandbeginselId, o => o.MapFrom(s => s.WoonlandbeginselId))
            .ForMember(d => d.TijdvakId, o => o.MapFrom(s => s.TijdvakId))
            .ForMember(d => d.InkomenWit, o => o.MapFrom(s => s.InkomenWit))
            .ForMember(d => d.InkomenGroen, o => o.MapFrom(s => s.InkomenGroen))

            .ForMember(d => d.KlantId, o => o.MapFrom(s => s.Werkgever.Klant.KlantId))
            .ForMember(d => d.WerkgeverId, o => o.MapFrom(s => s.Werkgever.Id))
            .ForMember(d => d.EmployeeId, o => o.MapFrom(s=>s.EmployeeId))
            .ForMember(d => d.LoonOverVanaf, o => o.MapFrom(s => s.LoonOverVanaf))
            .ForMember(d => d.LoonOverTot, o => o.MapFrom(s => s.LoonOverTot))
            .ForMember(d => d.LoonInVanaf, o => o.MapFrom(s => s.LoonInVanaf))
            .ForMember(d => d.LoonInTot, o => o.MapFrom(s => s.LoonInTot))
            .ForMember(d => d.AlgemeneHeffingskortingToegepast, o => o.MapFrom(s => s.AlgemeneHeffingskortingToegepast))

            .ForMember(d => d.BasisDagen, o => o.MapFrom(s => s.BasisDagen))
            .ForMember(d => d.InhoudingOpLoonWit, o => o.MapFrom(s => s.InhoudingOpLoonWit))
            .ForMember(d => d.InhoudingOpLoonGroen, o => o.MapFrom(s => s.InhoudingOpLoonGroen))
            .ForMember(d => d.AlgemeneHeffingskortingBedrag, o => o.MapFrom(s => s.AlgemeneHeffingskortingBedrag))
            .ForMember(d => d.VerrekendeArbeIdskorting, o => o.MapFrom(s => s.VerrekendeArbeIdskorting))
            .ForMember(d => d.SociaalVerzekeringsloon, o => o.MapFrom(s => s.SociaalVerzekeringsloon))
            .ForMember(d => d.PremieBedragAlgemeenWerkloosheIdsFondsLaagHoog,
                o => o.MapFrom(s => s.PremieBedragAlgemeenWerkloosheIdsFondsLaagHoog))
            .ForMember(d => d.PremieBedragAlgemeenWerkloosheIdsFondsLaag,
                o => o.MapFrom(s => s.PremieBedragAlgemeenWerkloosheIdsFondsLaag))

            .ForMember(d => d.PremieBedragAlgemeenWerkloosheIdsFondsHoog,
                o => o.MapFrom(s => s.PremieBedragAlgemeenWerkloosheIdsFondsHoog))
            .ForMember(d => d.IsPremieBedragUitvoeringsFondsvoordeOverheId,
                o => o.MapFrom(s => s.IsPremieBedragUitvoeringsFondsvoordeOverheId))
            .ForMember(d => d.PremieBedragUitvoeringsFondsvoordeOverheId,
                o => o.MapFrom(s => s.PremieBedragUitvoeringsFondsvoordeOverheId))
            .ForMember(d => d.PremieBedragWetArbeIdsOngeschikheIdLaagHoog,
                o => o.MapFrom(s => s.PremieBedragWetArbeIdsOngeschikheIdLaagHoog))

            .ForMember(d => d.PremieBedragWetArbeIdsOngeschikheIdLaag,
                o => o.MapFrom(s => s.PremieBedragWetArbeIdsOngeschikheIdLaag))
            .ForMember(d => d.PremieBedragWetArbeIdsOngeschikheIdHoog,
                o => o.MapFrom(s => s.PremieBedragWetArbeIdsOngeschikheIdHoog))
            .ForMember(d => d.PremieBedragWetKinderopvang, o => o.MapFrom(s => s.PremieBedragWetKinderopvang))
            .ForMember(d => d.PremieBedragZiektekostenVerzekeringsWetLoon,
                o => o.MapFrom(s => s.PremieBedragZiektekostenVerzekeringsWetLoon))
            .ForMember(d => d.Payee, o => o.MapFrom(s => s.Payee))

            .ForMember(d => d.PremieBedragZiektekostenVerzekeringsWetWerkgeversbijdrage,
                o => o.MapFrom(s => s.PremieBedragZiektekostenVerzekeringsWetWerkgeversbijdrage))
            .ForMember(d => d.PremieBedragZiektekostenVerzekeringsWetWerknemersbijdrage,
                o => o.MapFrom(s => s.PremieBedragZiektekostenVerzekeringsWetWerknemersbijdrage))
            .ForMember(d => d.WerkgeverWHKPremieBedragWGAVastWerkgever,
                o => o.MapFrom(s => s.WerkgeverWHKPremieBedragWGAVastWerkgever))
            .ForMember(d => d.WerkgeverWHKPremieBedragWGAVastWerknemer,
                o => o.MapFrom(s => s.WerkgeverWHKPremieBedragWGAVastWerknemer))
            .ForMember(d => d.WerkgeverWHKPremieBedragFlexWerkgever,
                o => o.MapFrom(s => s.WerkgeverWHKPremieBedragFlexWerkgever))
            .ForMember(d => d.WerkgeverWHKPremieBedragFlexWerknemer,
                o => o.MapFrom(s => s.WerkgeverWHKPremieBedragFlexWerknemer))
            .ForMember(d => d.WerkgeverWHKPremieBedragZWFlex, o => o.MapFrom(s => s.WerkgeverWHKPremieBedragZWFlex))
            .ForMember(d => d.WerkgeverWHKPremieBedragTotaal, o => o.MapFrom(s => s.WerkgeverWHKPremieBedragTotaal))
            .ForMember(d => d.NettoTeBetalenSubTotaal, o => o.MapFrom(s => s.NettoTeBetalenSubTotaal))
            .ForMember(d => d.NettoTeBetalenEindTotaal, o => o.MapFrom(s => s.NettoTeBetalenEindTotaal))
            .ForMember(d => d.Deleted, o => o.MapFrom(s => s.Deleted))
            .ForMember(d => d.Actief, o => o.MapFrom(s => s.Actief));
    }
}