using Lab.Gym.Web.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab.Gym.Web.Repository.Data.Contexts
{
    public class GymWebDbContext : DbContext
    {
        public DbSet<ScheduleEvent> ScheduleEvents { get; set; }

        public GymWebDbContext(DbContextOptions<GymWebDbContext> options) : base(options)
        { }
    }
}
