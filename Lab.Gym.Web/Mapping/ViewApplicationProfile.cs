using AutoMapper;
using Lab.Gym.Web.Pages.Manage.Profile;
using Lab.Gym.Web.Application.Models;
using Lab.Gym.Web.Pages.Schedule;
using Lab.Gym.Web.Domain.Models;
using Lab.Gym.Web.Application.Features.ScheduleEvents.Commands;
using System.Globalization;

namespace Lab.Gym.Web.Mapping
{
    public class ViewApplicationProfile: Profile
    {
        public ViewApplicationProfile()
        {
            CreateMap<InputModel, UserProfile>().ReverseMap();
            //.ForMember((d) => d.Email, opts => opts.MapFrom(src => src.Email));

            CreateMap<ScheduleEventModel, ScheduleEventVm>().ReverseMap();
            CreateMap<ScheduleEventVm, CreateRequest>()
                .ForMember((d) => d.Id, opts => opts.Ignore())
                .ForMember((d) => d.Start, opts => opts.MapFrom<CustomResolver>())
                ;

            CreateMap<ScheduleEventVm, UpdateRequest>()
                .ForMember((d) => d.Id, opts => opts.MapFrom(src => new Guid(src.Id)));
            CreateMap<ScheduleEventVm, DeleteRequest>().ForMember((d) => d.Id, opts => opts.MapFrom(src => new Guid(src.Id)));
        }
    }

    public class CustomResolver : IValueResolver<ScheduleEventVm, CreateRequest, DateTime>
    {
        public DateTime Resolve(ScheduleEventVm source, CreateRequest destination, DateTime member, ResolutionContext context)
        {
            if(string.IsNullOrEmpty(source.Start))
                throw new Exception("source.Start IS EMPTY OR NULL");

            return DateTime.ParseExact(source.Start, "dd/MM/yyyy h:mm t", CultureInfo.InvariantCulture);
        }
    }
}
