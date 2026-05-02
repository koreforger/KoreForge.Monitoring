namespace KoreForge.Monitoring.Contracts.Registry;

/// <summary>
/// Request used by an application instance to register itself with the monitoring registry.
/// </summary>
/// <param name="ApplicationInstanceId">Stable identifier for this monitored application instance.</param>
/// <param name="ApplicationName">Human-readable application name.</param>
/// <param name="ApplicationType">Application family or role.</param>
/// <param name="InstanceId">Runtime instance identifier.</param>
/// <param name="Environment">Deployment environment name.</param>
/// <param name="Version">Application version.</param>
/// <param name="MonitoringBaseUrl">Base URL for this instance's monitoring endpoints.</param>
/// <param name="Labels">Optional labels for filtering and display.</param>
public sealed record MonitoringAppRegistrationDto(
    string ApplicationInstanceId,
    string ApplicationName,
    string ApplicationType,
    string InstanceId,
    string Environment,
    string Version,
    string MonitoringBaseUrl,
    IReadOnlyDictionary<string, string>? Labels = null);

/// <summary>
/// Request used by an application instance to refresh its registry lease.
/// </summary>
/// <param name="ApplicationInstanceId">Stable identifier for this monitored application instance.</param>
/// <param name="Status">Optional current status text.</param>
/// <param name="Version">Optional current application version.</param>
public sealed record MonitoringAppHeartbeatDto(
    string ApplicationInstanceId,
    string? Status = null,
    string? Version = null);

/// <summary>
/// Registry entry returned to the shell for a monitored application instance.
/// </summary>
/// <param name="ApplicationInstanceId">Stable identifier for this monitored application instance.</param>
/// <param name="ApplicationName">Human-readable application name.</param>
/// <param name="ApplicationType">Application family or role.</param>
/// <param name="InstanceId">Runtime instance identifier.</param>
/// <param name="Environment">Deployment environment name.</param>
/// <param name="Version">Application version.</param>
/// <param name="MonitoringBaseUrl">Base URL for this instance's monitoring endpoints.</param>
/// <param name="Status">Current registry status.</param>
/// <param name="RegisteredAtUtc">UTC timestamp when the instance registered.</param>
/// <param name="LastSeenUtc">UTC timestamp of the latest heartbeat.</param>
/// <param name="ExpiresAtUtc">UTC timestamp when the registry lease expires.</param>
/// <param name="Labels">Labels for filtering and display.</param>
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

/// <summary>
/// Registry response containing the currently known monitored application instances.
/// </summary>
/// <param name="TimestampUtc">UTC timestamp when the list was produced.</param>
/// <param name="Apps">Known application instances.</param>
public sealed record MonitoringAppRegistryListDto(
    DateTimeOffset TimestampUtc,
    IReadOnlyList<MonitoringAppRegistryEntryDto> Apps);
