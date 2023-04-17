using AutoMapper;
using Prechart.Service.Belastingen.Database.Models;
using Prechart.Service.Globals.Interfaces.Belastingen;

namespace Prechart.Service.Belastingen.Models.Mapping;

public class BelastingenMapper : Profile
{
    public BelastingenMapper()
    {
        CreateMap<ITaxRecord, White>()
            .ForMember(d => d.Year, o => o.MapFrom(s => s.Year))
            .ForMember(d => d.CountryId, o => o.MapFrom(s => s.CountryId))
            .ForMember(d => d.TypeId, o => o.MapFrom(s => s.TypeId))
            .ForMember(d => d.Tabelloon, o => o.MapFrom(s => s.Tabelloon))
            .ForMember(d => d.ZonderLoonheffingskorting, o => o.MapFrom(s => s.ZonderLoonheffingskorting))
            .ForMember(d => d.MetLoonheffingskorting, o => o.MapFrom(s => s.MetLoonheffingskorting))
            .ForMember(d => d.VerrekendeArbeidskorting, o => o.MapFrom(s => s.VerrekendeArbeidskorting))
            .ForMember(d => d.EerderZonderLoonheffingskorting, o => o.MapFrom(s => s.EerderZonderLoonheffingskorting))
            .ForMember(d => d.EerderMetLoonheffingskorting, o => o.MapFrom(s => s.EerderMetLoonheffingskorting))
            .ForMember(d => d.EerderVerrekendeArbeidskorting, o => o.MapFrom(s => s.EerderVerrekendeArbeidskorting))
            .ForMember(d => d.LaterZonderLoonheffingskorting, o => o.MapFrom(s => s.LaterZonderLoonheffingskorting))
            .ForMember(d => d.LaterMetLoonheffingskorting, o => o.MapFrom(s => s.LaterMetLoonheffingskorting))
            .ForMember(d => d.LaterVerrekendeArbeidskorting, o => o.MapFrom(s => s.LaterVerrekendeArbeidskorting))
            .ForMember(d => d.Active, o => o.MapFrom(s => s.Active))
            .ReverseMap();

        CreateMap<ITaxRecord, Green>()
            .ForMember(d => d.Year, o => o.MapFrom(s => s.Year))
            .ForMember(d => d.CountryId, o => o.MapFrom(s => s.CountryId))
            .ForMember(d => d.TypeId, o => o.MapFrom(s => s.TypeId))
            .ForMember(d => d.Tabelloon, o => o.MapFrom(s => s.Tabelloon))
            .ForMember(d => d.ZonderLoonheffingskorting, o => o.MapFrom(s => s.ZonderLoonheffingskorting))
            .ForMember(d => d.MetLoonheffingskorting, o => o.MapFrom(s => s.MetLoonheffingskorting))
            .ForMember(d => d.VerrekendeArbeidskorting, o => o.MapFrom(s => s.VerrekendeArbeidskorting))
            .ForMember(d => d.EerderZonderLoonheffingskorting, o => o.MapFrom(s => s.EerderZonderLoonheffingskorting))
            .ForMember(d => d.EerderMetLoonheffingskorting, o => o.MapFrom(s => s.EerderMetLoonheffingskorting))
            .ForMember(d => d.EerderVerrekendeArbeidskorting, o => o.MapFrom(s => s.EerderVerrekendeArbeidskorting))
            .ForMember(d => d.LaterZonderLoonheffingskorting, o => o.MapFrom(s => s.LaterZonderLoonheffingskorting))
            .ForMember(d => d.LaterMetLoonheffingskorting, o => o.MapFrom(s => s.LaterMetLoonheffingskorting))
            .ForMember(d => d.LaterVerrekendeArbeidskorting, o => o.MapFrom(s => s.LaterVerrekendeArbeidskorting))
            .ForMember(d => d.Active, o => o.MapFrom(s => s.Active))
            .ReverseMap();
    }
}