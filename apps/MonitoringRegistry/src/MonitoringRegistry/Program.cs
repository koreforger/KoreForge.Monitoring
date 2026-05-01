using KF.Monitoring.Registry;

var builder = WebApplication.CreateBuilder(args);

var options = new MonitoringRegistryOptions();
builder.Configuration.GetSection("KoreForge:Monitoring:Registry").Bind(options);

builder.Services.AddSingleton(options);
builder.Services.AddSingleton<IMonitoringRegistryStore, InMemoryMonitoringRegistryStore>();
builder.Services.AddCors(cors =>
{
    cors.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

app.UseCors();
app.MapGet("/", () => Results.Ok(new { service = "KoreForge Monitoring Registry", status = "Running" }));
app.MapKoreForgeMonitoringRegistry();

app.Run();

/// <summary>
/// Entry point marker used by integration tests.
/// </summary>
public partial class Program;
