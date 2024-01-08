using AutoMapper;
using Lab.Core.IdentityServer.Pages.Manage.Profile;
using Lab.Gym.Web.Application.Models;

namespace Lab.Gym.Web.Mapping
{
    public class MainProfile: Profile
    {
        public MainProfile()
        {
            CreateMap<InputModel, UserProfile>().ReverseMap();
                //.ForMember((d) => d.Email, opts => opts.MapFrom(src => src.Email));
        }
    }
}
