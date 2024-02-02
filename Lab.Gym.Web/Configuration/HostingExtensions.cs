using IdentityModel.Client;
using Lab.Gym.Web.Application.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using Polly;
using System.Text;
using Lab.Gym.Web.Repository.Configuration;
using Serilog;
using Lab.Gym.Web.Repository;
using Lab.Gym.Web.Application;

namespace Lab.Gym.Web.Configuration;

public static class HostingExtensions
{
    public static IHostBuilder ConfigureSerilog(this IHostBuilder host)
    {
        host.UseSerilog((ctx, lc) => lc
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
            .Enrich.FromLogContext()
            .ReadFrom.Configuration(ctx.Configuration));

        return host;
    }

    public static IHostBuilder ConfigureAppSettings(this IHostBuilder host)
    {
        var enviroment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        Log.Information($"ASPNETCORE_ENVIRONMENT: {enviroment}");
        host.ConfigureAppConfiguration((ctx, builder) =>
        {
            builder.AddJsonFile("appsettings.json", false, true);
            builder.AddJsonFile($"appsettings.{enviroment}.json", true, true);
            builder.AddEnvironmentVariables();
        });

        return host;
    }

    public static void ConfigureInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorPages();
        builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

        builder.Services.AddAutoMapper(new[] { typeof(Program), typeof(IRepositoryModule), typeof(IApplicationModule) });
        builder.Services.AddFeatures();
        builder.Services.AddRepositories(builder.Configuration);
        builder.Services.AddDataProtectionFromContext();

        var identityBaseUrl = builder.Configuration["IdentityServer:BaseUrl"];
        Log.Information($"identityBaseUrl: {identityBaseUrl}");
        builder.Services.AddServices(identityBaseUrl);
    }

    public static void ConfigureAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies", options =>
            {
                options.Cookie.Name = "GymWebCookie";

                options.Events.OnSigningOut = async e =>
                {
                    // revoke refresh token on sign-out 
                    await e.HttpContext.RevokeUserRefreshTokenAsync();
                };
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = bool.Parse(builder.Configuration["RequireHttpsMetadata"]);
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            })
            .AddOpenIdConnect("oidc", options =>
            {
                options.RequireHttpsMetadata = bool.Parse(builder.Configuration["RequireHttpsMetadata"]);
                options.Authority = builder.Configuration["IdentityServer:BaseUrl"];
                options.ClientId = builder.Configuration["IdentityServer:OpenIdConnect:ClientId"];
                options.ClientSecret = builder.Configuration["IdentityServer:OpenIdConnect:ClientSecret"];
                options.ResponseType = "code";

                options.SaveTokens = true;

                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");
                options.Scope.Add("api1");

                // keeps id_token smaller
                //options.GetClaimsFromUserInfoEndpoint = true;
                options.SaveTokens = true;

                //options.ClaimActions.MapUniqueJsonKey("address1", "address1");
                //options.ClaimActions.MapUniqueJsonKey("fullname", "fullname");
                options.MapInboundClaims = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };
            });

        builder.Services
            .AddAccessTokenManagement(options =>
            {
                options.Client.Clients.Add("identityApi", new ClientCredentialsTokenRequest
                {
                    RequestUri = new Uri(new Uri(builder.Configuration["IdentityServer:BaseUrl"]), new Uri("/connect/token", UriKind.Relative)),
                    ClientId = builder.Configuration["IdentityServer:ClientCredentials:ClientId"],
                    ClientSecret = builder.Configuration["IdentityServer:ClientCredentials:ClientSecret"]
                });
            })
            .ConfigureBackchannelHttpClient()
            .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(new[]
            {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(3)
            }));
    }
}
