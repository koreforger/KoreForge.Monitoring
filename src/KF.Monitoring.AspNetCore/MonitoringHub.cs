using KF.Monitoring.AspNetCore.Providers;
using KF.Monitoring.Contracts.Metrics;
using Microsoft.AspNetCore.SignalR;

namespace KF.Monitoring.AspNetCore;

/// <summary>
/// SignalR hub for live monitoring interactions.
/// </summary>
public sealed class MonitoringHub : Hub
{
    private readonly IMonitoringMetricsProvider _metricsProvider;

    /// <summary>
    /// Creates a monitoring hub.
    /// </summary>
    /// <param name="metricsProvider">Metrics provider.</param>
    public MonitoringHub(IMonitoringMetricsProvider metricsProvider)
    {
        _metricsProvider = metricsProvider;
    }

    /// <summary>
    /// Gets the current metrics snapshot over SignalR.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The current metrics snapshot.</returns>
    public async Task<MetricSnapshotDto> GetSnapshot(CancellationToken cancellationToken)
    {
        return await _metricsProvider.GetSnapshotAsync(cancellationToken).ConfigureAwait(false);
    }
}
