using System.Collections.Concurrent;
using KF.Monitoring.Contracts.Registry;

namespace KF.Monitoring.Registry;

/// <summary>
/// Thread-safe in-memory implementation of the monitoring registry store.
/// </summary>
public sealed class InMemoryMonitoringRegistryStore : IMonitoringRegistryStore
{
    private readonly ConcurrentDictionary<string, MonitoringAppRegistryEntryDto> _entries = new(StringComparer.OrdinalIgnoreCase);
    private readonly MonitoringRegistryOptions _options;
    private readonly Func<DateTimeOffset> _utcNow;

    /// <summary>
    /// Creates an in-memory registry store.
    /// </summary>
    /// <param name="options">Registry options.</param>
    /// <param name="utcNow">Optional clock used by tests.</param>
    public InMemoryMonitoringRegistryStore(MonitoringRegistryOptions options, Func<DateTimeOffset>? utcNow = null)
    {
        _options = options;
        _utcNow = utcNow ?? (() => DateTimeOffset.UtcNow);
    }

    /// <inheritdoc />
    public Task<MonitoringAppRegistryEntryDto> RegisterAsync(
        MonitoringAppRegistrationDto registration,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var now = _utcNow();
        var entry = new MonitoringAppRegistryEntryDto(
            registration.ApplicationInstanceId,
            registration.ApplicationName,
            registration.ApplicationType,
            registration.InstanceId,
            registration.Environment,
            registration.Version,
            registration.MonitoringBaseUrl.TrimEnd('/'),
            "Unknown",
            now,
            now,
            now.Add(_options.HeartbeatTtl),
            registration.Labels is null
                ? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                : new Dictionary<string, string>(registration.Labels, StringComparer.OrdinalIgnoreCase));

        _entries[entry.ApplicationInstanceId] = entry;

        return Task.FromResult(entry);
    }

    /// <inheritdoc />
    public Task<MonitoringAppRegistryEntryDto?> HeartbeatAsync(
        MonitoringAppHeartbeatDto heartbeat,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!_entries.TryGetValue(heartbeat.ApplicationInstanceId, out var existing))
        {
            return Task.FromResult<MonitoringAppRegistryEntryDto?>(null);
        }

        var now = _utcNow();
        var updated = existing with
        {
            Status = heartbeat.Status ?? existing.Status,
            Version = heartbeat.Version ?? existing.Version,
            LastSeenUtc = now,
            ExpiresAtUtc = now.Add(_options.HeartbeatTtl),
        };

        _entries[updated.ApplicationInstanceId] = updated;

        return Task.FromResult<MonitoringAppRegistryEntryDto?>(updated);
    }

    /// <inheritdoc />
    public Task<MonitoringAppRegistryListDto> ListAsync(bool includeExpired, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var now = _utcNow();
        var apps = _entries.Values
            .Where(entry => includeExpired || entry.ExpiresAtUtc > now)
            .OrderBy(entry => entry.ApplicationName, StringComparer.OrdinalIgnoreCase)
            .ThenBy(entry => entry.InstanceId, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return Task.FromResult(new MonitoringAppRegistryListDto(now, apps));
    }

    /// <inheritdoc />
    public Task<MonitoringAppRegistryEntryDto?> GetAsync(
        string applicationInstanceId,
        bool includeExpired,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!_entries.TryGetValue(applicationInstanceId, out var entry))
        {
            return Task.FromResult<MonitoringAppRegistryEntryDto?>(null);
        }

        if (!includeExpired && entry.ExpiresAtUtc <= _utcNow())
        {
            return Task.FromResult<MonitoringAppRegistryEntryDto?>(null);
        }

        return Task.FromResult<MonitoringAppRegistryEntryDto?>(entry);
    }
}
