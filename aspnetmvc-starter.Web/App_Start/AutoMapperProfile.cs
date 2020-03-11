using aspnetmvc_starter.Main.Core.Domain;
using aspnetmvc_starter.Web.Dtos;
using AutoMapper;
using aspnetmvc_starter.Web.Dtos;
using aspnetmvc_starter.Web.ViewModels;

namespace aspnetmvc_starter.Web
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            //CreateMap<User, UserViewModel>().ReverseMap();

        }

        public static IMapper Init()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile()));

            return new Mapper(configuration);
        }

    }
}