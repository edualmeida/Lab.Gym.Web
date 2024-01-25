
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using Polly;
using System.Text;
using Lab.Gym.Web.Application.Extensions;
using Lab.Gym.Web.Repository.Configuration;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddRazorPages();
builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddFeatures();
builder.Services.AddRepositories(builder.Configuration);

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
        //options.RequireHttpsMetadata = false;
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

builder.Services.AddServices(builder.Configuration["IdentityServer:BaseUrl"]);

var app = builder.Build();

//app.MigrationInitialisation();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var forwardOptions = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
    RequireHeaderSymmetry = false
};

forwardOptions.KnownNetworks.Clear();
forwardOptions.KnownProxies.Clear();

// ref: https://github.com/aspnet/Docs/issues/2384
app.UseForwardedHeaders(forwardOptions);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
