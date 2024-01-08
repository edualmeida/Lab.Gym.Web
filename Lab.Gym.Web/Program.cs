
using IdentityModel.Client;
using Lab.Gym.Web.Application.Clients;
using Lab.Gym.Web.Application.Services;
using Microsoft.IdentityModel.Tokens;
using Refit;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddAccessTokenManagement(options =>
{
    options.Client.Clients.Add("identityApi", new ClientCredentialsTokenRequest
    {
        RequestUri = new Uri(new Uri("https://localhost:5001"), new Uri("/connect/token", UriKind.Relative)),
        ClientId = "identityApi",
        ClientSecret = "secret"
    });
});

builder.Services
    .AddRefitClient<IProfileApiClient>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:5001"))
    .AddClientAccessTokenHandler("identityApi");

builder.Services.AddScoped<IProfileService, ProfileService>();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookies";
        options.DefaultChallengeScheme = "oidc";
    })
    .AddCookie("Cookies")
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
        options.Authority = "https://localhost:5001";

        options.ClientId = "mvc";
        options.ClientSecret = "secret";
        options.ResponseType = "code";

        options.SaveTokens = true;

        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");
        options.Scope.Add("api1");

        //options.GetClaimsFromUserInfoEndpoint = true;
        //options.ClaimActions.MapUniqueJsonKey("address1", "address1");
        //options.ClaimActions.MapUniqueJsonKey("fullname", "fullname");

        options.MapInboundClaims = false;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
