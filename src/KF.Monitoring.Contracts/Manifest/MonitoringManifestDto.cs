using KF.Monitoring.Contracts.Health;
using KF.Monitoring.Contracts.Metrics;
using KF.Monitoring.Contracts.Process;

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
    ProcessDefinitionDto? Process,
    MetricSnapshotDto? MetricsSnapshot,
    string? Stream,
    HealthSnapshotDto? Health);

public sealed record CapabilityDefinitionDto(
    string Key,
    string DisplayName,
    string Component,
    string Endpoint,
    IReadOnlyDictionary<string, object?>? Config = null);
