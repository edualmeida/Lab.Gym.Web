using AutoMapper;
using Lab.Gym.Web.Pages.Manage.Profile;
using Lab.Gym.Web.Application.Models;
using Lab.Gym.Web.Pages.Schedule;
using Lab.Gym.Web.Domain.Models;
using Lab.Gym.Web.Application.Features.ScheduleEvents.Commands;

namespace Lab.Gym.Web.Mapping
{
    public class MainProfile: Profile
    {
        public MainProfile()
        {
            CreateMap<InputModel, UserProfile>().ReverseMap();
            //.ForMember((d) => d.Email, opts => opts.MapFrom(src => src.Email));

            CreateMap<ScheduleEventModel, ScheduleEventVm>().ReverseMap();
            CreateMap<ScheduleEventVm, CreateRequest>()
                .ForMember((d) => d.Id, opts => opts.Ignore());
            CreateMap<ScheduleEventVm, UpdateRequest>()
                .ForMember((d) => d.Id, opts => opts.MapFrom(src => new Guid(src.Id)));
            CreateMap<ScheduleEventVm, DeleteRequest>().ForMember((d) => d.Id, opts => opts.MapFrom(src => new Guid(src.Id)));
        }
    }
}
