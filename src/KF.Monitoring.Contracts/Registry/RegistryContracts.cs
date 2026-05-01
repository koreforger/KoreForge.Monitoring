using KF.Monitoring.Contracts.Health;
using KF.Monitoring.Contracts.Manifest;

namespace KF.Monitoring.Contracts.Registry;

public sealed record MonitoringAppRegistrationDto(
    string ApplicationId,
    string ApplicationName,
    string ApplicationType,
    string InstanceId,
    string Environment,
    string Version,
    string ManifestEndpoint,
    string? StreamEndpoint = null,
    DateTimeOffset? RegisteredAt = null);

public sealed record MonitoringAppHeartbeatDto(
    string ApplicationId,
    string InstanceId,
    DateTimeOffset Timestamp,
    MonitoringHealthStatus Status,
    string? Version = null,
    IReadOnlyDictionary<string, object?>? Data = null);

public sealed record MonitoringAppRegistryEntryDto(
    string ApplicationId,
    string ApplicationName,
    string ApplicationType,
    string InstanceId,
    string Environment,
    string Version,
    string ManifestEndpoint,
    string? StreamEndpoint,
    DateTimeOffset RegisteredAt,
    DateTimeOffset LastHeartbeatAt,
    MonitoringHealthStatus Status,
    MonitoringManifestDto? Manifest = null);

public sealed record MonitoringAppRegistryListDto(
    IReadOnlyList<MonitoringAppRegistryEntryDto> Entries,
    DateTimeOffset Timestamp);
