using Lab.Gym.Web.Repository.Data.Contexts;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lab.Gym.Web.Repository.Configuration
{
    public static class HostingExtensions
    {
        public static void AddDataProtectionFromContext(this IServiceCollection services)
        {
            services.AddDataProtection().PersistKeysToDbContext<GymWebDbContext>();
        }

        public static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<GymWebDbContext>(
                options => options.UseNpgsql(configuration.GetConnectionString("GymWebConnectionString")));

            services.AddScoped<IScheduleEventRepository, ScheduleEventRepository>();

        }
    }
}
