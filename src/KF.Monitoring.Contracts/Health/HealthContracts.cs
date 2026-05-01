using System.Text.Json.Serialization;

namespace KF.Monitoring.Contracts.Health;

public sealed record HealthSnapshotDto(
    MonitoringHealthStatus Status,
    IReadOnlyList<HealthCheckDto> Checks);

public sealed record HealthCheckDto(
    string Name,
    MonitoringHealthStatus Status,
    string Description);

public sealed record HealthChangedDto(
    string MessageType,
    DateTimeOffset TimestampUtc,
    string ApplicationId,
    MonitoringHealthStatus OverallStatus,
    IReadOnlyList<HealthCheckDto> Changed);

[JsonConverter(typeof(JsonStringEnumConverter<MonitoringHealthStatus>))]
public enum MonitoringHealthStatus
{
    Healthy,
    Degraded,
    Unhealthy,
    Unknown
}
