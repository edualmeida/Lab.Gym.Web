using Lab.Gym.Web.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace Lab.Gym.Web.Application.Configuration
{
    public static class HostingExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IUserAccountService, UserAccountService>();

            services
                .AddHttpClient<IUserAccountService, UserAccountService>(httpClient =>
                {
                    httpClient.BaseAddress = new Uri("https://localhost:5001/");
                })
                .AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.WaitAndRetryAsync(3, retryNumber => TimeSpan.FromSeconds(30)))
                .AddClientAccessTokenHandler();
        }
    }
}
