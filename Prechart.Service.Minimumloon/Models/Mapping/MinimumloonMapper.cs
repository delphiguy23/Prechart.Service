using AutoMapper;

namespace Prechart.Service.Minimumloon.Models.Mapping;

public class MinimumloonMapper : Profile
{
    public MinimumloonMapper()
    {
        CreateMap<MinimumloonModel, Database.Models.Minimumloon>()
                       .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                       .ForMember(d => d.Jaar, o => o.MapFrom(s => s.Jaar))
                       .ForMember(d => d.MinimumloonLeeftijd, o => o.MapFrom(s => s.MinimumloonLeeftijd))
                       .ForMember(d => d.MinimumloonPerMaand, o => o.MapFrom(s => s.MinimumloonPerMaand))
                       .ForMember(d => d.MinimumloonPerWeek, o => o.MapFrom(s => s.MinimumloonPerWeek))
                       .ForMember(d => d.MinimumloonPerDag, o => o.MapFrom(s => s.MinimumloonPerDag))
                       .ForMember(d => d.MinimumloonPerUur36, o => o.MapFrom(s => s.MinimumloonPerUur36))
                       .ForMember(d => d.MinimumloonPerUur38, o => o.MapFrom(s => s.MinimumloonPerUur38))
                       .ForMember(d => d.MinimumloonPerUur40, o => o.MapFrom(s => s.MinimumloonPerUur40))
                       .ForMember(d => d.MinimumloonRecordActief, o => o.MapFrom(s => s.MinimumloonRecordActief))
                       .ForMember(d => d.MinimumloonRecordActiefVanaf, o => o.MapFrom(s => s.MinimumloonRecordActiefVanaf))
                       .ForMember(d => d.MinimumloonRecordActiefTot, o => o.MapFrom(s => s.MinimumloonRecordActiefTot))
                       .ReverseMap();
    }
}
