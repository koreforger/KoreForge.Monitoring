using KF.Monitoring.Contracts.Registry;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace KF.Monitoring.Registry;

/// <summary>
/// Endpoint mapping helpers for the monitoring registry API.
/// </summary>
public static class MonitoringRegistryEndpointRouteBuilderExtensions
{
    /// <summary>
    /// Maps the monitoring registry endpoints under the supplied route prefix.
    /// </summary>
    /// <param name="endpoints">Endpoint route builder.</param>
    /// <param name="prefix">Route prefix for registry endpoints.</param>
    /// <returns>The same endpoint route builder.</returns>
    public static IEndpointRouteBuilder MapKoreForgeMonitoringRegistry(
        this IEndpointRouteBuilder endpoints,
        string prefix = "/registry")
    {
        var group = endpoints.MapGroup(prefix);

        group.MapPost("/apps/register", async (
            MonitoringAppRegistrationDto registration,
            IMonitoringRegistryStore store,
            CancellationToken cancellationToken) =>
            Results.Ok(await store.RegisterAsync(registration, cancellationToken).ConfigureAwait(false)));

        group.MapPost("/apps/{applicationInstanceId}/heartbeat", async (
            string applicationInstanceId,
            MonitoringAppHeartbeatDto heartbeat,
            IMonitoringRegistryStore store,
            CancellationToken cancellationToken) =>
        {
            var request = heartbeat with { ApplicationInstanceId = applicationInstanceId };
            var updated = await store.HeartbeatAsync(request, cancellationToken).ConfigureAwait(false);

            return updated is null ? Results.NotFound() : Results.Ok(updated);
        });

        group.MapGet("/apps", async (
            bool? includeExpired,
            IMonitoringRegistryStore store,
            CancellationToken cancellationToken) =>
            Results.Ok(await store.ListAsync(includeExpired == true, cancellationToken).ConfigureAwait(false)));

        group.MapGet("/apps/{applicationInstanceId}", async (
            string applicationInstanceId,
            bool? includeExpired,
            IMonitoringRegistryStore store,
            CancellationToken cancellationToken) =>
        {
            var entry = await store.GetAsync(applicationInstanceId, includeExpired == true, cancellationToken).ConfigureAwait(false);

            return entry is null ? Results.NotFound() : Results.Ok(entry);
        });

        return endpoints;
    }
}
