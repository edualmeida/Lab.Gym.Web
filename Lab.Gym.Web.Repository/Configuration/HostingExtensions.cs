using Lab.Gym.Web.Repository.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lab.Gym.Web.Repository.Configuration
{
    public static class HostingExtensions
    {
        public static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(IAppModule));

            services.AddDbContext<GymWebDbContext>(
                options => options.UseNpgsql(configuration.GetConnectionString("GymWebConnectionString")));

            services.AddScoped<IScheduleEventRepository, ScheduleEventRepository>();

        }
    }
}
