using System.Text.Json.Serialization;

namespace KoreForge.Monitoring.Contracts.Health;

/// <summary>
/// Captures the current health state reported by an application instance.
/// </summary>
/// <param name="Status">Overall health status.</param>
/// <param name="Checks">Individual health checks contributing to the status.</param>
public sealed record HealthSnapshotDto(
    MonitoringHealthStatus Status,
    IReadOnlyList<HealthCheckDto> Checks);

/// <summary>
/// Describes one health check result.
/// </summary>
/// <param name="Name">Health check name.</param>
/// <param name="Status">Health check status.</param>
/// <param name="Description">Human-readable status detail.</param>
public sealed record HealthCheckDto(
    string Name,
    MonitoringHealthStatus Status,
    string Description);

/// <summary>
/// SignalR payload emitted when health state changes.
/// </summary>
/// <param name="MessageType">Message discriminator for the live stream.</param>
/// <param name="TimestampUtc">UTC timestamp for the change.</param>
/// <param name="ApplicationId">Application identifier that emitted the change.</param>
/// <param name="OverallStatus">Current overall health status.</param>
/// <param name="Changed">Health checks that changed.</param>
public sealed record HealthChangedDto(
    string MessageType,
    DateTimeOffset TimestampUtc,
    string ApplicationId,
    MonitoringHealthStatus OverallStatus,
    IReadOnlyList<HealthCheckDto> Changed);

/// <summary>
/// Health status values used by monitoring endpoints and live events.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<MonitoringHealthStatus>))]
public enum MonitoringHealthStatus
{
    /// <summary>All checks are healthy.</summary>
    Healthy,
    /// <summary>The application is running with impaired behavior.</summary>
    Degraded,
    /// <summary>The application is not healthy.</summary>
    Unhealthy,
    /// <summary>The health status is not known.</summary>
    Unknown
}
