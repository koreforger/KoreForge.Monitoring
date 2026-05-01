using KF.Monitoring.Contracts.Registry;

namespace KF.Monitoring.Registry;

/// <summary>
/// Stores registrations and heartbeat state for monitored application instances.
/// </summary>
public interface IMonitoringRegistryStore
{
    /// <summary>
    /// Registers or replaces an application instance.
    /// </summary>
    /// <param name="registration">Registration request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The stored registry entry.</returns>
    Task<MonitoringAppRegistryEntryDto> RegisterAsync(
        MonitoringAppRegistrationDto registration,
        CancellationToken cancellationToken);

    /// <summary>
    /// Refreshes an application instance lease.
    /// </summary>
    /// <param name="heartbeat">Heartbeat request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated entry, or null when the instance is unknown.</returns>
    Task<MonitoringAppRegistryEntryDto?> HeartbeatAsync(
        MonitoringAppHeartbeatDto heartbeat,
        CancellationToken cancellationToken);

    /// <summary>
    /// Lists registered application instances.
    /// </summary>
    /// <param name="includeExpired">Whether expired entries should be included.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A registry list response.</returns>
    Task<MonitoringAppRegistryListDto> ListAsync(
        bool includeExpired,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets one application instance by identifier.
    /// </summary>
    /// <param name="applicationInstanceId">Application instance identifier.</param>
    /// <param name="includeExpired">Whether expired entries should be returned.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The matching entry, or null when it is unknown or expired.</returns>
    Task<MonitoringAppRegistryEntryDto?> GetAsync(
        string applicationInstanceId,
        bool includeExpired,
        CancellationToken cancellationToken);
}
