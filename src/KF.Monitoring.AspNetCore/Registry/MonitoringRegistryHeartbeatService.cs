using System.Net.Http.Json;
using KF.Monitoring.Contracts.Registry;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KF.Monitoring.AspNetCore.Registry;

/// <summary>
/// Background service that registers an application with a monitoring registry and sends heartbeats.
/// </summary>
public sealed class MonitoringRegistryHeartbeatService : BackgroundService
{
    private readonly HttpClient _httpClient;
    private readonly MonitoringAppRegistrationDto _registration;
    private readonly TimeSpan _interval;
    private readonly ILogger<MonitoringRegistryHeartbeatService> _logger;

    /// <summary>
    /// Creates a monitoring registry heartbeat service.
    /// </summary>
    /// <param name="httpClient">HTTP client configured for the registry base address.</param>
    /// <param name="registration">Application registration payload.</param>
    /// <param name="interval">Heartbeat interval.</param>
    /// <param name="logger">Logger.</param>
    public MonitoringRegistryHeartbeatService(
        HttpClient httpClient,
        MonitoringAppRegistrationDto registration,
        TimeSpan interval,
        ILogger<MonitoringRegistryHeartbeatService> logger)
    {
        _httpClient = httpClient;
        _registration = registration;
        _interval = interval;
        _logger = logger;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await RegisterAsync(stoppingToken).ConfigureAwait(false);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(_interval, stoppingToken).ConfigureAwait(false);
            await HeartbeatAsync(stoppingToken).ConfigureAwait(false);
        }
    }

    private async Task RegisterAsync(CancellationToken cancellationToken)
    {
        var response = await _httpClient
            .PostAsJsonAsync("/registry/apps/register", _registration, cancellationToken)
            .ConfigureAwait(false);

        _logger.LogInformation("Monitoring registry registration returned {StatusCode}", response.StatusCode);
    }

    private async Task HeartbeatAsync(CancellationToken cancellationToken)
    {
        var heartbeat = new MonitoringAppHeartbeatDto(
            _registration.ApplicationInstanceId,
            "Unknown",
            _registration.Version);

        var response = await _httpClient
            .PostAsJsonAsync($"/registry/apps/{_registration.ApplicationInstanceId}/heartbeat", heartbeat, cancellationToken)
            .ConfigureAwait(false);

        _logger.LogDebug("Monitoring registry heartbeat returned {StatusCode}", response.StatusCode);
    }
}
