using AutoMapper;
using Prechart.Service.Core.Extensions;
using Prechart.Service.Globals.Interfaces.Werkgever;
using Prechart.Service.Globals.Models.Werkgever;
using Prechart.Service.Werkgever.Database.Models;

namespace Prechart.Service.Werkgever.Models.Mapping;

public class WerkgeverMapper : Profile
{
    public WerkgeverMapper()
    {
        CreateMap<MongoWerkgeverModel, IMongoWerkgever>().ReverseMap();

        CreateMap<NewWerkgeverModel, Database.Models.Werkgever>()
            .ForMember(d => d.WerkgeverMongoId, o => o.MapFrom(s => s.WerkgeverMongoId))
            .ForMember(d => d.KlantMongoId, o => o.MapFrom(s => s.KlantMongoId))
            .ForMember(d => d.Naam, o => o.MapFrom(s => s.Naam))
            .ForMember(d => d.Sector, o => o.MapFrom(s => s.Sector))
            .ForMember(d => d.FiscaalNummer, o => o.MapFrom(s => s.FiscaalNummer))
            .ForMember(d => d.LoonheffingenExtentie, o => o.MapFrom(s => s.LoonheffingenExtentie))
            .ForMember(d => d.OmzetbelastingExtentie, o => o.MapFrom(s => s.OmzetbelastingExtentie))
            .ForMember(d => d.DatumActiefVanaf, o => o.MapFrom(s => s.DatumActiefVanaf))
            .ForMember(d => d.DatumActiefTot, o => o.MapFrom(s => s.DatumActiefTot))
            .ForMember(d => d.Actief, o => o.MapFrom(s => s.Actief))
            .ForMember(d => d.Id, o => o.Ignore());

        CreateMap<NewWerkgeverWhkPremies, WerkgeverWhkPremies>()
            .ForMember(d => d.WerkgeverId, o => o.MapFrom(s => s.WerkgeverId))
            .ForMember(d => d.WgaVastWerkgever, o => o.MapFrom(s => s.WgaVastWerkgever))
            .ForMember(d => d.WgaVastWerknemer, o => o.MapFrom(s => s.WgaVastWerknemer))
            .ForMember(d => d.FlexWerkgever, o => o.MapFrom(s => s.FlexWerkgever))
            .ForMember(d => d.FlexWerknemer, o => o.MapFrom(s => s.FlexWerknemer))
            .ForMember(d => d.ZwFlex, o => o.MapFrom(s => s.ZwFlex))
            .ForMember(d => d.Totaal, o => o.MapFrom(s => s.Totaal))
            .ForMember(d => d.ActiefVanaf, o => o.MapFrom(s => s.ActiefVanaf))
            .ForMember(d => d.ActiefTot, o => o.MapFrom(s => s.ActiefTot))
            .ForMember(d => d.Actief, o => o.MapFrom(s => s.Actief))
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.WerkgeverWhkMongoId, o => o.Ignore())
            .ReverseMap();

        CreateMap<MongoWerkgeverModel, Database.Models.Werkgever>()
            .ForMember(d => d.WerkgeverMongoId, o => o.MapFrom(s => s.Id.ToString()))
            .ForMember(d => d.KlantMongoId, o => o.MapFrom(s => s.Klant.KlantId))
            .ForMember(d => d.Naam, o => o.MapFrom(s => s.Naam))
            .ForMember(d => d.Sector, o => o.MapFrom(s => s.Sector))
            .ForMember(d => d.FiscaalNummer, o => o.MapFrom(s => s.FiscaalNummer))
            .ForMember(d => d.LoonheffingenExtentie, o => o.MapFrom(s => s.LoonheffingenExtentie))
            .ForMember(d => d.OmzetbelastingExtentie, o => o.MapFrom(s => s.OmzetbelastingExtentie))
            .ForMember(d => d.DatumActiefVanaf, o => o.MapFrom(s => s.DatumActiefVanaf))
            .ForMember(d => d.DatumActiefTot, o => o.MapFrom(s => s.DatumActiefTot))
            .ForMember(d => d.Actief, o => o.MapFrom(s => s.Actief))
            .ForMember(d => d.Id, o => o.Ignore());

        CreateMap<Database.Models.Werkgever, MongoWerkgeverModel>()
            .ForMember(d => d.Naam, o => o.MapFrom(s => s.Naam))
            .ForMember(d => d.Sector, o => o.MapFrom(s => s.Sector))
            .ForMember(d => d.FiscaalNummer, o => o.MapFrom(s => s.FiscaalNummer))
            .ForMember(d => d.LoonheffingenExtentie, o => o.MapFrom(s => s.LoonheffingenExtentie))
            .ForMember(d => d.OmzetbelastingExtentie, o => o.MapFrom(s => s.OmzetbelastingExtentie))
            .ForMember(d => d.DatumActiefVanaf, o => o.MapFrom(s => s.DatumActiefVanaf))
            .ForMember(d => d.DatumActiefTot, o => o.MapFrom(s => s.DatumActiefTot))
            .ForMember(d => d.Actief, o => o.MapFrom(s => s.Actief))
            .ForMember(d => d.Id, o => o.MapFrom(s => s.WerkgeverMongoId.ToObjectId()))
            .ForMember(d => d.Collectieve, o => o.Ignore())
            .ForMember(d => d.Klant, o => o.Ignore())
            .ForMember(d => d.WhkPremies, o => o.Ignore())
            .ForMember(d => d.DateCreated, o => o.Ignore())
            .ForMember(d => d.DateLastModified, o => o.Ignore());


        CreateMap<MongoWhkPremie, WerkgeverWhkPremies>()
            .ForMember(d => d.WerkgeverWhkMongoId, o => o.MapFrom(s => s.Id.ToString()))
            .ForMember(d => d.WgaVastWerkgever, o => o.MapFrom(s => s.WgaVastWerkgever))
            .ForMember(d => d.WgaVastWerknemer, o => o.MapFrom(s => s.WgaVastWerknemer))
            .ForMember(d => d.FlexWerkgever, o => o.MapFrom(s => s.FlexWerkgever))
            .ForMember(d => d.FlexWerknemer, o => o.MapFrom(s => s.FlexWerknemer))
            .ForMember(d => d.ZwFlex, o => o.MapFrom(s => s.ZwFlex))
            .ForMember(d => d.ActiefVanaf, o => o.MapFrom(s => s.ActiefVanaf))
            .ForMember(d => d.ActiefTot, o => o.MapFrom(s => s.ActiefTot))
            .ForMember(d => d.Actief, o => o.MapFrom(s => s.Actief))
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.WerkgeverId, o => o.Ignore());

        CreateMap<WerkgeverWhkPremies, MongoWhkPremie>()
            .ForMember(d => d.WgaVastWerkgever, o => o.MapFrom(s => s.WgaVastWerkgever))
            .ForMember(d => d.WgaVastWerknemer, o => o.MapFrom(s => s.WgaVastWerknemer))
            .ForMember(d => d.FlexWerkgever, o => o.MapFrom(s => s.FlexWerkgever))
            .ForMember(d => d.FlexWerknemer, o => o.MapFrom(s => s.FlexWerknemer))
            .ForMember(d => d.ZwFlex, o => o.MapFrom(s => s.ZwFlex))
            .ForMember(d => d.ActiefVanaf, o => o.MapFrom(s => s.ActiefVanaf))
            .ForMember(d => d.ActiefTot, o => o.MapFrom(s => s.ActiefTot))
            .ForMember(d => d.Actief, o => o.MapFrom(s => s.Actief))
            .ForMember(d => d.SqlId, o => o.MapFrom(s => s.Id))
            .ForMember(d => d.Id, o => o.MapFrom(s => s.WerkgeverWhkMongoId.ToObjectId()))
            .ForMember(d => d.DateCreated, o => o.Ignore())
            .ForMember(d => d.DateLastModified, o => o.Ignore());
    }
}
