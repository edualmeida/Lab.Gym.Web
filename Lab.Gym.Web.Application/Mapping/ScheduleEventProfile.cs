using AutoMapper;
using Lab.Gym.Web.Application.Features.ScheduleEvents.Commands;
using Lab.Gym.Web.Repository.Models;

namespace Lab.Gym.Web.Application.Mapping
{
    public class ScheduleEventProfile: Profile
    {
        public ScheduleEventProfile()
        {
            CreateMap<CreateRequest, ScheduleEvent>();
            CreateMap<UpdateRequest, ScheduleEvent>();
        }
    }
}
