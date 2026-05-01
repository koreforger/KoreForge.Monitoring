namespace KF.Monitoring.Contracts.Registry;

public sealed record MonitoringAppRegistrationDto(
    string ApplicationInstanceId,
    string ApplicationName,
    string ApplicationType,
    string InstanceId,
    string Environment,
    string Version,
    string MonitoringBaseUrl,
    IReadOnlyDictionary<string, string>? Labels = null);

public sealed record MonitoringAppHeartbeatDto(
    string ApplicationInstanceId,
    string? Status = null,
    string? Version = null);

public sealed record MonitoringAppRegistryEntryDto(
    string ApplicationInstanceId,
    string ApplicationName,
    string ApplicationType,
    string InstanceId,
    string Environment,
    string Version,
    string MonitoringBaseUrl,
    string Status,
    DateTimeOffset RegisteredAtUtc,
    DateTimeOffset LastSeenUtc,
    DateTimeOffset ExpiresAtUtc,
    IReadOnlyDictionary<string, string> Labels);

public sealed record MonitoringAppRegistryListDto(
    DateTimeOffset TimestampUtc,
    IReadOnlyList<MonitoringAppRegistryEntryDto> Apps);
