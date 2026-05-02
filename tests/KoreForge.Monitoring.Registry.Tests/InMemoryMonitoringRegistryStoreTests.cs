using KoreForge.Monitoring.Contracts.Registry;
using Xunit;

namespace KoreForge.Monitoring.Registry.Tests;

public sealed class InMemoryMonitoringRegistryStoreTests
{
    [Fact]
    public async Task RegisterAsync_NewApp_AddsEntry()
    {
        var now = DateTimeOffset.Parse("2026-04-30T12:00:00Z");
        var store = new InMemoryMonitoringRegistryStore(
            new MonitoringRegistryOptions { HeartbeatTtl = TimeSpan.FromSeconds(30) },
            () => now);

        var entry = await store.RegisterAsync(new MonitoringAppRegistrationDto(
            "eventreader-prod-01",
            "EventReader",
            "EventReader",
            "prod-01",
            "PROD",
            "1.4.7",
            "https://eventreader"), CancellationToken.None);

        Assert.Equal("eventreader-prod-01", entry.ApplicationInstanceId);
        Assert.Equal(now.AddSeconds(30), entry.ExpiresAtUtc);
    }

    [Fact]
    public async Task HeartbeatAsync_ExistingApp_UpdatesLastSeenAndStatus()
    {
        var current = DateTimeOffset.Parse("2026-04-30T12:00:00Z");
        var store = new InMemoryMonitoringRegistryStore(
            new MonitoringRegistryOptions { HeartbeatTtl = TimeSpan.FromSeconds(30) },
            () => current);
        await store.RegisterAsync(
            new MonitoringAppRegistrationDto("a", "App", "Type", "1", "DEV", "1.0", "http://app"),
            CancellationToken.None);

        current = current.AddSeconds(10);
        var updated = await store.HeartbeatAsync(new MonitoringAppHeartbeatDto("a", "Degraded", "1.1"), CancellationToken.None);

        Assert.NotNull(updated);
        Assert.Equal("Degraded", updated!.Status);
        Assert.Equal("1.1", updated.Version);
        Assert.Equal(current, updated.LastSeenUtc);
    }

    [Fact]
    public async Task ListAsync_ExpiredApp_IsNotReturned()
    {
        var current = DateTimeOffset.Parse("2026-04-30T12:00:00Z");
        var store = new InMemoryMonitoringRegistryStore(
            new MonitoringRegistryOptions { HeartbeatTtl = TimeSpan.FromSeconds(5) },
            () => current);
        await store.RegisterAsync(
            new MonitoringAppRegistrationDto("a", "App", "Type", "1", "DEV", "1.0", "http://app"),
            CancellationToken.None);

        current = current.AddSeconds(6);
        var list = await store.ListAsync(includeExpired: false, CancellationToken.None);

        Assert.Empty(list.Apps);
    }
}
