using AutoMapper;
using Lab.Gym.Web.Repository.Models;

namespace Lab.Gym.Web.Repository
{
    public class MainProfile: Profile
    {
        public MainProfile()
        {
            CreateMap<ScheduleEvent, ScheduleEvent>();
        }
    }
}
