using DockForge.Components;
using DockForge.Core.Interfaces;
using DockForge.Services.Docker;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var useFakeDocker = builder.Configuration.GetValue("Docker:UseFakeData", true);

if (useFakeDocker)
{
    builder.Services.AddScoped<IDockerService, FakeDockerService>();
}
else
{
    builder.Services.AddScoped<IDockerService, DockerEngineService>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
