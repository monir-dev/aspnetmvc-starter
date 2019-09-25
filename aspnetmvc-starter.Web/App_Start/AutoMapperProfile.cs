using aspnetmvc_starter.Main.Core.Domain;
using aspnetmvc_starter.Web.Dtos;
using AutoMapper;

namespace aspnetmvc_starter.Web
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
        }

        public static IMapper Init()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile()));
            
            return new Mapper(configuration);
        }
    }
}