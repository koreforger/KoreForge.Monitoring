using KF.Monitoring.Contracts.Health;
using KF.Monitoring.Contracts.Manifest;
using KF.Monitoring.Contracts.Metrics;
using KF.Monitoring.Contracts.Process;

namespace KF.Monitoring.AspNetCore.Providers;

/// <summary>
/// Supplies the monitoring manifest for an application instance.
/// </summary>
public interface IMonitoringManifestProvider
{
    /// <summary>
    /// Gets the monitoring manifest.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The monitoring manifest.</returns>
    Task<MonitoringManifestDto> GetManifestAsync(CancellationToken cancellationToken);
}

/// <summary>
/// Supplies process definitions for an application instance.
/// </summary>
public interface IMonitoringProcessProvider
{
    /// <summary>
    /// Gets a process definition.
    /// </summary>
    /// <param name="processId">Optional child process identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The requested process definition.</returns>
    Task<ProcessDefinitionDto> GetProcessAsync(string? processId, CancellationToken cancellationToken);
}

/// <summary>
/// Supplies metric snapshots for an application instance.
/// </summary>
public interface IMonitoringMetricsProvider
{
    /// <summary>
    /// Gets the current metric snapshot.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The current metric snapshot.</returns>
    Task<MetricSnapshotDto> GetSnapshotAsync(CancellationToken cancellationToken);
}

/// <summary>
/// Supplies health state for an application instance.
/// </summary>
public interface IMonitoringHealthProvider
{
    /// <summary>
    /// Gets the current health snapshot.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The current health snapshot.</returns>
    Task<HealthSnapshotDto> GetHealthAsync(CancellationToken cancellationToken);
}
