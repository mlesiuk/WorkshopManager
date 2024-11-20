using workshopManager.Presentation.Components;
using workshopManager.Presentation.Models;
using workshopManager.Presentation.Services;

var builder = WebApplication.CreateBuilder(args);

var configuration = new Configuration();
builder.Configuration
    .GetSection(nameof(Configuration))
    .Bind(configuration);

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient("workshopManagerApi", conf =>
{
    conf.BaseAddress = new Uri(configuration.ApiUrl);
    conf.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddScoped<IApiService, ApiService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
