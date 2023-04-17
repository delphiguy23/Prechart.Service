using AutoMapper;

namespace Prechart.Service.Users.Models.Mapping;

public class UsersMapper : Profile
{
    public UsersMapper()
    {
        // CreateMap<UserModel, User.Database.Models.Users>()
        //     .ForMember(d => d.Id, o => o.Ignore())
        //     .ReverseMap();
    }
}