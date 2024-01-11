using Lab.Gym.Web.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace Lab.Gym.Web.Application.Extensions
{
    public static class HostingExtensions
    {
        public static void AddFeatures(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IAppModule).Assembly));
        }

        public static void AddServices(this IServiceCollection services, string baseUrl)
        {
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IUserAccountService, UserAccountService>();

            services.AddIdentityHttpClients<IUserAccountService, UserAccountService>(baseUrl);
            services.AddIdentityHttpClients<IProfileService, ProfileService>(baseUrl);
        }

        private static void AddIdentityHttpClients<TClient, TImplementation>(this IServiceCollection services, string baseUrl)
            where TClient : class
            where TImplementation : class, TClient
        {
            services
                .AddHttpClient<TClient, TImplementation>(httpClient => { httpClient.BaseAddress = new Uri(baseUrl); })
                .AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.WaitAndRetryAsync(3, retryNumber => TimeSpan.FromSeconds(30)))
                .AddClientAccessTokenHandler()
                ;
        }
    }
}
