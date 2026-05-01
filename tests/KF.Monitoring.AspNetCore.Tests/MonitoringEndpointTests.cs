using System.Net.Http.Json;
using KF.Monitoring.AspNetCore.Providers;
using KF.Monitoring.Contracts.Health;
using KF.Monitoring.Contracts.Manifest;
using KF.Monitoring.Contracts.Metrics;
using KF.Monitoring.Contracts.Process;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace KF.Monitoring.AspNetCore.Tests;

public sealed class MonitoringEndpointTests
{
    [Fact]
    public async Task MonitoringEndpoints_ReturnStandardContracts()
    {
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();
        builder.Services.AddSignalR();
        builder.Services.AddSingleton<IMonitoringManifestProvider, FakeManifestProvider>();
        builder.Services.AddSingleton<IMonitoringProcessProvider, FakeProcessProvider>();
        builder.Services.AddSingleton<IMonitoringMetricsProvider, FakeMetricsProvider>();
        builder.Services.AddSingleton<IMonitoringHealthProvider, FakeHealthProvider>();

        await using var app = builder.Build();
        app.MapKoreForgeMonitoring();
        await app.StartAsync();

        using var client = app.GetTestClient();

        var manifest = await client.GetFromJsonAsync<MonitoringManifestDto>("/monitoring/manifest");
        var process = await client.GetFromJsonAsync<ProcessDefinitionDto>("/monitoring/process");
        var metrics = await client.GetFromJsonAsync<MetricSnapshotDto>("/monitoring/metrics/snapshot");
        var health = await client.GetFromJsonAsync<HealthSnapshotDto>("/monitoring/health");

        Assert.Equal("test-01", manifest!.ApplicationId);
        Assert.Equal("test-process", process!.ProcessId);
        Assert.Equal("test-01", metrics!.ApplicationId);
        Assert.Equal(MonitoringHealthStatus.Healthy, health!.Status);
    }

    private sealed class FakeManifestProvider : IMonitoringManifestProvider
    {
        public Task<MonitoringManifestDto> GetManifestAsync(CancellationToken cancellationToken) =>
            Task.FromResult(new MonitoringManifestDto(
                "test-01",
                "TestApp",
                "Test",
                "01",
                "DEV",
                "1.0",
                "1.0",
                [],
                "/monitoring/process",
                "/monitoring/metrics/snapshot",
                "/monitoring/hub",
                "/monitoring/health"));
    }

    private sealed class FakeProcessProvider : IMonitoringProcessProvider
    {
        public Task<ProcessDefinitionDto> GetProcessAsync(string? processId, CancellationToken cancellationToken) =>
            Task.FromResult(new ProcessDefinitionDto(
                "test-process",
                "Test",
                "1",
                [new ProcessStepDto("read", "READ", ProcessStepKind.Source, "read", "test.read")],
                []));
    }

    private sealed class FakeMetricsProvider : IMonitoringMetricsProvider
    {
        public Task<MetricSnapshotDto> GetSnapshotAsync(CancellationToken cancellationToken) =>
            Task.FromResult(new MetricSnapshotDto(
                DateTimeOffset.UtcNow,
                "test-01",
                [new MetricValueDto("test.read.inputRate", "Input", MetricKind.Rate, 1, "records/sec", "read")]));
    }

    private sealed class FakeHealthProvider : IMonitoringHealthProvider
    {
        public Task<HealthSnapshotDto> GetHealthAsync(CancellationToken cancellationToken) =>
            Task.FromResult(new HealthSnapshotDto(
                MonitoringHealthStatus.Healthy,
                [new HealthCheckDto("test", MonitoringHealthStatus.Healthy, "ok")]));
    }
}
