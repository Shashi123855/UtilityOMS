using UtilityOMS.Web.Components;
using UtilityOMS.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// ✅ Add Blazor Web App services
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ✅ HttpClient to talk to the API
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7033/")
});

// ✅ Our frontend services
builder.Services.AddScoped<OutageClientService>();
builder.Services.AddScoped<ResidentClientService>();
builder.Services.AddScoped<CrewClientService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();