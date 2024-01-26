using Microsoft.AspNetCore.HttpOverrides;
using Lab.Gym.Web.Configuration;

var builder = WebApplication.CreateBuilder(args);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Host.ConfigureSerilog();
builder.Host.ConfigureAppSettings();
builder.ConfigureInfrastructure();
builder.ConfigureAuthentication();

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
