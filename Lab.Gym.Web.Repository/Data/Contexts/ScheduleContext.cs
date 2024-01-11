using Lab.Gym.Web.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab.Gym.Web.Repository.Data.Contexts
{
    public class ScheduleContext : DbContext
    {
        public DbSet<ScheduleEventRepo> ScheduleEvents { get; set; }
    }
}
