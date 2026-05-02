namespace KoreForge.Monitoring.Contracts.Manifest;

/// <summary>
/// Describes the monitoring surface exposed by one running application instance.
/// </summary>
/// <param name="ApplicationId">Stable logical application identifier.</param>
/// <param name="ApplicationName">Human-readable application name.</param>
/// <param name="ApplicationType">Application family or role, such as EventReader.</param>
/// <param name="InstanceId">Runtime instance identifier for this process/container.</param>
/// <param name="Environment">Deployment environment name.</param>
/// <param name="Version">Application version reported by the instance.</param>
/// <param name="MonitoringContractVersion">Version of the monitoring contract implemented by the instance.</param>
/// <param name="Capabilities">Application-specific capabilities and panels exposed by the instance.</param>
/// <param name="Process">Endpoint path for the top-level process definition.</param>
/// <param name="MetricsSnapshot">Endpoint path for the current metrics snapshot.</param>
/// <param name="Stream">SignalR hub path for live monitoring updates.</param>
/// <param name="Health">Endpoint path for health state.</param>
public sealed record MonitoringManifestDto(
    string ApplicationId,
    string ApplicationName,
    string ApplicationType,
    string InstanceId,
    string Environment,
    string Version,
    string MonitoringContractVersion,
    IReadOnlyList<CapabilityDefinitionDto> Capabilities,
    string Process,
    string MetricsSnapshot,
    string Stream,
    string Health);

/// <summary>
/// Declares an optional capability that the shell can render with a registered panel component.
/// </summary>
/// <param name="Key">Stable capability key used by process steps and panel registration.</param>
/// <param name="DisplayName">Human-readable capability name.</param>
/// <param name="Component">Frontend component registration key.</param>
/// <param name="Endpoint">Endpoint path used to load capability data.</param>
/// <param name="Config">Optional component configuration values.</param>
public sealed record CapabilityDefinitionDto(
    string Key,
    string DisplayName,
    string Component,
    string Endpoint,
    IReadOnlyDictionary<string, object?>? Config = null);
