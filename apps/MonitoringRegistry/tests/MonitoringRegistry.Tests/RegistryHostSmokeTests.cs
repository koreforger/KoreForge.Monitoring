using System.Net.Http.Json;
using KoreForge.Monitoring.Contracts.Registry;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace MonitoringRegistry.Tests;

public sealed class RegistryHostSmokeTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public RegistryHostSmokeTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task RegistryHost_RegisterAndList_ReturnsApp()
    {
        var client = _factory.CreateClient();
        var registration = new MonitoringAppRegistrationDto(
            "eventreader-dev-01",
            "EventReader",
            "EventReader",
            "dev-01",
            "DEV",
            "1.0.0",
            "http://localhost:5000");

        var registerResponse = await client.PostAsJsonAsync("/registry/apps/register", registration);
        registerResponse.EnsureSuccessStatusCode();

        var list = await client.GetFromJsonAsync<MonitoringAppRegistryListDto>("/registry/apps");

        Assert.NotNull(list);
        Assert.Contains(list!.Apps, app => app.ApplicationInstanceId == "eventreader-dev-01");
    }
}
