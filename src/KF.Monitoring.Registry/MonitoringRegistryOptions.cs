namespace KF.Monitoring.Registry;

/// <summary>
/// Options for the monitoring registry service.
/// </summary>
public sealed class MonitoringRegistryOptions
{
    /// <summary>
    /// Time a registered application remains visible without another heartbeat.
    /// </summary>
    public TimeSpan HeartbeatTtl { get; set; } = TimeSpan.FromSeconds(45);

    /// <summary>
    /// Optional shared API key for internal deployments that want simple registry protection.
    /// </summary>
    public string? ApiKey { get; set; }
}
