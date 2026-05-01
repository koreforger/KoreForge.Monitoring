namespace KF.Monitoring.Contracts.Manifest;

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

public sealed record CapabilityDefinitionDto(
    string Key,
    string DisplayName,
    string Component,
    string Endpoint,
    IReadOnlyDictionary<string, object?>? Config = null);
