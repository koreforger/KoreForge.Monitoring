using KF.Monitoring.AspNetCore.Providers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace KF.Monitoring.AspNetCore;

/// <summary>
/// Endpoint mapping helpers for monitored application instances.
/// </summary>
public static class MonitoringEndpointRouteBuilderExtensions
{
    /// <summary>
    /// Maps standard KoreForge monitoring endpoints and the SignalR hub.
    /// </summary>
    /// <param name="endpoints">Endpoint route builder.</param>
    /// <param name="prefix">Route prefix for monitoring endpoints.</param>
    /// <returns>The same endpoint route builder.</returns>
    public static IEndpointRouteBuilder MapKoreForgeMonitoring(
        this IEndpointRouteBuilder endpoints,
        string prefix = "/monitoring")
    {
        var group = endpoints.MapGroup(prefix);

        group.MapGet("/manifest", async (IMonitoringManifestProvider provider, CancellationToken cancellationToken) =>
            Results.Ok(await provider.GetManifestAsync(cancellationToken).ConfigureAwait(false)));

        group.MapGet("/process", async (IMonitoringProcessProvider provider, CancellationToken cancellationToken) =>
            Results.Ok(await provider.GetProcessAsync(null, cancellationToken).ConfigureAwait(false)));

        group.MapGet("/processes/{processId}", async (
            string processId,
            IMonitoringProcessProvider provider,
            CancellationToken cancellationToken) =>
            Results.Ok(await provider.GetProcessAsync(processId, cancellationToken).ConfigureAwait(false)));

        group.MapGet("/metrics/snapshot", async (IMonitoringMetricsProvider provider, CancellationToken cancellationToken) =>
            Results.Ok(await provider.GetSnapshotAsync(cancellationToken).ConfigureAwait(false)));

        group.MapGet("/health", async (IMonitoringHealthProvider provider, CancellationToken cancellationToken) =>
            Results.Ok(await provider.GetHealthAsync(cancellationToken).ConfigureAwait(false)));

        group.MapHub<MonitoringHub>("/hub");

        return endpoints;
    }
}
