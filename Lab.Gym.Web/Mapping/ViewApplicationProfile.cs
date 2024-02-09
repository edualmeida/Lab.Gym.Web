using AutoMapper;
using Lab.Gym.Web.Pages.Manage.Profile;
using Lab.Gym.Web.Application.Models;
using Lab.Gym.Web.Pages.Schedule;
using Lab.Gym.Web.Domain.Models;
using Lab.Gym.Web.Application.Features.ScheduleEvents.Commands;
using System.Globalization;
using Lab.Gym.Web.Constants;

namespace Lab.Gym.Web.Mapping
{
    public class ViewApplicationProfile: Profile
    {
        public ViewApplicationProfile()
        {
            CreateMap<InputModel, UserProfile>().ReverseMap();
            //.ForMember((d) => d.Email, opts => opts.MapFrom(src => src.Email));

            CreateMap<ScheduleEventModel, ScheduleEventVm>()
                .ForMember((d) => d.Start, opts => opts.MapFrom<ApplicationStartResolver>())
                .ForMember((d) => d.End, opts => opts.MapFrom<ApplicationEndResolver>());

            CreateMap<ScheduleEventVm, CreateRequest>()
                .ForMember((d) => d.Id, opts => opts.Ignore())
                .ForMember((d) => d.Start, opts => opts.MapFrom<ViewModelStartResolver>())
                .ForMember((d) => d.End, opts => opts.MapFrom<ViewModelEndResolver>())
                ;

            CreateMap<ScheduleEventVm, UpdateRequest>()
                .ForMember((d) => d.Id, opts => opts.MapFrom(src => new Guid(src.Id)))
                .ForMember((d) => d.Start, opts => opts.MapFrom<ViewModelStartResolver>())
                .ForMember((d) => d.End, opts => opts.MapFrom<ViewModelEndResolver>())
                ;

            CreateMap<ScheduleEventVm, DeleteRequest>().ForMember((d) => d.Id, opts => opts.MapFrom(src => new Guid(src.Id)));
        }

        internal class ViewModelStartResolver : IValueResolver<ScheduleEventVm, ScheduleEventModel, DateTime>
        {
            public DateTime Resolve(ScheduleEventVm source, ScheduleEventModel destination, DateTime member, ResolutionContext context)
            {
                if (string.IsNullOrEmpty(source.Start))
                    throw new Exception("Start IS EMPTY OR NULL");

                return DateTime.ParseExact(source.Start, FormatConstants.SourceDateFormat, new CultureInfo("en-IE"), DateTimeStyles.None);
            }
        }

        internal class ViewModelEndResolver : IValueResolver<ScheduleEventVm, ScheduleEventModel, DateTime?>
        {
            public DateTime? Resolve(ScheduleEventVm source, ScheduleEventModel destination, DateTime? member, ResolutionContext context)
            {
                if (string.IsNullOrEmpty(source.End))
                    return null;

                return DateTime.ParseExact(source.End, FormatConstants.SourceDateFormat, new CultureInfo("en-IE"), DateTimeStyles.None);
            }
        }

        internal class ApplicationStartResolver : IValueResolver<ScheduleEventModel, ScheduleEventVm, string>
        {
            public string Resolve(ScheduleEventModel source, ScheduleEventVm destination, string member, ResolutionContext context)
            {
                return source.Start.ToString(FormatConstants.SourceDateFormat);
            }
        }

        internal class ApplicationEndResolver : IValueResolver<ScheduleEventModel, ScheduleEventVm, string?>
        {
            public string? Resolve(ScheduleEventModel source, ScheduleEventVm destination, string? member, ResolutionContext context)
            {
                if (!source.End.HasValue)
                    return "";

                return source.End.Value.ToString(FormatConstants.SourceDateFormat);
            }
        }
    }
}
