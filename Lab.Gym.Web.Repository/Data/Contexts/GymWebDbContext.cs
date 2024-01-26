using Lab.Gym.Web.Repository.Models;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lab.Gym.Web.Repository.Data.Contexts
{
    public class GymWebDbContext : DbContext, IDataProtectionKeyContext
    {
        DbSet<DataProtectionKey> IDataProtectionKeyContext.DataProtectionKeys { get { return DataProtectionKeys; } }
        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = null!;
        public DbSet<ScheduleEvent> ScheduleEvents { get; set; }

        public GymWebDbContext(DbContextOptions<GymWebDbContext> options) : base(options)
        { }
    }
}
