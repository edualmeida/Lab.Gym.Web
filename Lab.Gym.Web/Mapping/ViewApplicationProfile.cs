using AutoMapper;
using Lab.Gym.Web.Pages.Manage.Profile;
using Lab.Gym.Web.Application.Models;
using Lab.Gym.Web.Pages.Schedule;
using Lab.Gym.Web.Domain.Models;
using Lab.Gym.Web.Application.Features.ScheduleEvents.Commands;
using System.Globalization;
using Lab.Gym.Web.Repository.Models;

namespace Lab.Gym.Web.Mapping
{
    public class ViewApplicationProfile: Profile
    {
        private static string _sourceDateTimeFormat = "dd/MM/yyyy HH:mm";

        public ViewApplicationProfile()
        {
            CreateMap<InputModel, UserProfile>().ReverseMap();
            //.ForMember((d) => d.Email, opts => opts.MapFrom(src => src.Email));

            CreateMap<ScheduleEventModel, ScheduleEventVm>().ReverseMap();
            CreateMap<ScheduleEventVm, CreateRequest>()
                .ForMember((d) => d.Id, opts => opts.Ignore())
                .ForMember((d) => d.Start, opts => opts.MapFrom<CustomStartResolver>())
                .ForMember((d) => d.End, opts => opts.MapFrom<CustomEndResolver>())
                ;

            CreateMap<ScheduleEventVm, UpdateRequest>()
                .ForMember((d) => d.Id, opts => opts.MapFrom(src => new Guid(src.Id)));
            CreateMap<ScheduleEventVm, DeleteRequest>().ForMember((d) => d.Id, opts => opts.MapFrom(src => new Guid(src.Id)));
        }

        internal class CustomStartResolver : IValueResolver<ScheduleEventVm, CreateRequest, DateTime>
        {
            public DateTime Resolve(ScheduleEventVm source, CreateRequest destination, DateTime member, ResolutionContext context)
            {
                if (string.IsNullOrEmpty(source.Start))
                    throw new Exception("Start IS EMPTY OR NULL");

                return DateTime.ParseExact(source.Start, _sourceDateTimeFormat, new CultureInfo("en-IE"), DateTimeStyles.None);
            }
        }

        internal class CustomEndResolver : IValueResolver<ScheduleEventVm, CreateRequest, DateTime?>
        {
            public DateTime? Resolve(ScheduleEventVm source, CreateRequest destination, DateTime? member, ResolutionContext context)
            {
                if (string.IsNullOrEmpty(source.End))
                    return null;

                return DateTime.ParseExact(source.End, _sourceDateTimeFormat, new CultureInfo("en-IE"), DateTimeStyles.None);
            }
        }
    }
}
