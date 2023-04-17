using AutoMapper;
using Prechart.Service.Globals.Interfaces.Loonheffings;

namespace Prechart.Service.Person.Models.Mapping
{
    public class PersonMapper : Profile
    {
        public PersonMapper()
        {
            CreateMap<PersonModel, IXmlToPerson>().ReverseMap();
        }
    }
}