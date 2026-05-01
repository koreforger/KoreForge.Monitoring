using System.Text.Json.Serialization;

namespace KF.Monitoring.Contracts.Health;

public sealed record HealthSnapshotDto(
    MonitoringHealthStatus Status,
    IReadOnlyList<HealthCheckDto> Checks,
    DateTimeOffset Timestamp);

public sealed record HealthCheckDto(
    string Key,
    string DisplayName,
    MonitoringHealthStatus Status,
    string? Description = null,
    DateTimeOffset? Timestamp = null,
    IReadOnlyDictionary<string, object?>? Data = null);

public sealed record HealthChangedDto(
    string ApplicationId,
    string InstanceId,
    MonitoringHealthStatus PreviousStatus,
    MonitoringHealthStatus CurrentStatus,
    DateTimeOffset Timestamp,
    IReadOnlyList<HealthCheckDto> Checks);

[JsonConverter(typeof(JsonStringEnumConverter<MonitoringHealthStatus>))]
public enum MonitoringHealthStatus
{
    Healthy,
    Degraded,
    Unhealthy,
    Unknown
}
