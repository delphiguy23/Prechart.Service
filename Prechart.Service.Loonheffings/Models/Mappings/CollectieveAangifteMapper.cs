using AutoMapper;
using Prechart.Service.Globals.Models.Loonheffings;
using Prechart.Service.Globals.Models.Loonheffings.Xsd.Xsd2022;

namespace Prechart.Service.Loonheffings.Models.Mappings;

public class CollectieveAangifteMapper : Profile
{
    public CollectieveAangifteMapper()
    {
        CreateMap<CollectieveAangifteTijdvakAangifteType, CollectieveAangifteModel>()
            .ForMember(d => d.TotLnLbPh, o => o.MapFrom(s => s.TotLnLbPh))
            .ForMember(d => d.TotLnSv, o => o.MapFrom(s => s.TotLnSV))
            .ForMember(d => d.TotPrlnAofAnwLg, o => o.MapFrom(s => s.TotPrlnAofAnwLg))
            .ForMember(d => d.TotPrlnAofAnwHg, o => o.MapFrom(s => s.TotPrlnAofAnwHg))
            .ForMember(d => d.TotPrlnAofAnwUit, o => o.MapFrom(s => s.TotPrlnAofAnwUit))
            .ForMember(d => d.TotPrlnAwfAnwLg, o => o.MapFrom(s => s.TotPrlnAwfAnwLg))
            .ForMember(d => d.TotPrlnAwfAnwHg, o => o.MapFrom(s => s.TotPrlnAwfAnwHg))
            .ForMember(d => d.TotPrlnAwfAnwHz, o => o.MapFrom(s => s.TotPrlnAwfAnwHz))
            .ForMember(d => d.PrLnUfo, o => o.MapFrom(s => s.PrLnUFO))
            .ForMember(d => d.IngLbPh, o => o.MapFrom(s => s.IngLbPh))
            .ForMember(d => d.EhPubUitk, o => o.MapFrom(s => s.EHPubUitk))
            .ForMember(d => d.EhGebrAuto, o => o.MapFrom(s => s.EHGebrAuto))
            .ForMember(d => d.EhVut, o => o.MapFrom(s => s.EHVUT))
            .ForMember(d => d.EhOvsFrfWrkkstrg, o => o.MapFrom(s => s.EhOvsFrfWrkkstrg))
            .ForMember(d => d.AvZeev, o => o.MapFrom(s => s.AVZeev))
            .ForMember(d => d.VrlAvso, o => o.MapFrom(s => s.VrlAVSO))
            .ForMember(d => d.TotPrAofLg, o => o.MapFrom(s => s.TotPrAofLg))
            .ForMember(d => d.TotPrAofHg, o => o.MapFrom(s => s.TotPrAofHg))
            .ForMember(d => d.TotPrAofUit, o => o.MapFrom(s => s.TotPrAofUit))
            .ForMember(d => d.TotOpslWko, o => o.MapFrom(s => s.TotOpslWko))
            .ForMember(d => d.TotPrGediffWhk, o => o.MapFrom(s => s.TotPrGediffWhk))
            .ForMember(d => d.TotPrAwfLg, o => o.MapFrom(s => s.TotPrAwfLg))
            .ForMember(d => d.TotPrAwfHg, o => o.MapFrom(s => s.TotPrAwfHg))
            .ForMember(d => d.TotPrAwfHz, o => o.MapFrom(s => s.TotPrAwfHz))
            .ForMember(d => d.PrUfo, o => o.MapFrom(s => s.PrUFO))
            .ForMember(d => d.IngBijdrZvw, o => o.MapFrom(s => s.IngBijdrZvw))
            .ForMember(d => d.TotWghZvw, o => o.MapFrom(s => s.TotWghZvw))
            .ForMember(d => d.TotTeBet, o => o.MapFrom(s => s.TotTeBet))
            .ForMember(d => d.TotGen, o => o.MapFrom(s => s.TotGen))
            .ForMember(d => d.SaldoCorrectiesVoorgaandTijdvak, o => o.MapFrom(s => s.SaldoCorrectiesVoorgaandTijdvak))
            .ReverseMap();

        CreateMap<CollectieveAangifteTijdvakAangifteTypeSaldoCorrectiesVoorgaandTijdvak, SaldoCorrectiesVoorgaandTijdvakModel>()
            .ForMember(d => d.DatAanvTv, o => o.MapFrom(s => s.DatAanvTv))
            .ForMember(d => d.DatEindTv, o => o.MapFrom(s => s.DatEindTv))
            .ForMember(d => d.Saldo, o => o.MapFrom(s => s.Saldo))
            .ReverseMap();
    }
}
