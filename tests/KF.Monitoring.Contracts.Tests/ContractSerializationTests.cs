using System.Text.Json;
using KF.Monitoring.Contracts.EventReader;
using KF.Monitoring.Contracts.Health;
using KF.Monitoring.Contracts.Manifest;
using KF.Monitoring.Contracts.Metrics;
using KF.Monitoring.Contracts.Process;
using KF.Monitoring.Contracts.Registry;
using Xunit;

namespace KF.Monitoring.Contracts.Tests;

public sealed class ContractSerializationTests
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    [Fact]
    public void MonitoringManifestDto_serializes_exact_task_contract_shape()
    {
        var manifest = new MonitoringManifestDto(
            ApplicationId: "app-1",
            ApplicationName: "Event Reader",
            ApplicationType: "worker",
            InstanceId: "instance-1",
            Environment: "dev",
            Version: "1.0.0",
            MonitoringContractVersion: "1",
            Capabilities:
            [
                new CapabilityDefinitionDto(
                    Key: "event-reader",
                    DisplayName: "Event Reader",
                    Component: "EventReader",
                    Endpoint: "/monitoring/event-reader",
                    Config: new Dictionary<string, object?> { ["enabled"] = true })
            ],
            Process: "/monitoring/process",
            MetricsSnapshot: "/monitoring/metrics",
            Stream: "/monitoring/stream",
            Health: "/monitoring/health");

        var json = JsonSerializer.Serialize(manifest, JsonOptions);

        Assert.Contains("\"applicationId\":\"app-1\"", json);
        Assert.Contains("\"monitoringContractVersion\":\"1\"", json);
        Assert.Contains("\"displayName\":\"Event Reader\"", json);
        Assert.Contains("\"process\":\"/monitoring/process\"", json);
        Assert.Contains("\"metricsSnapshot\":\"/monitoring/metrics\"", json);
        Assert.Contains("\"health\":\"/monitoring/health\"", json);
    }

    [Fact]
    public void ProcessDefinitionDto_serializes_child_process_endpoint_and_branch_flow()
    {
        var process = new ProcessDefinitionDto(
            ProcessId: "process-1",
            Name: "Import process",
            Version: "1.0.0",
            Steps:
            [
                new ProcessStepDto(
                    Id: "parent",
                    Label: "Parent",
                    Kind: ProcessStepKind.Process,
                    ComponentKey: "import",
                    MetricsPrefix: "process.import",
                    ChildProcessEndpoint: "/monitoring/processes/child",
                    CapabilityKey: "child-process",
                    MetricBindings: new StepMetricBindingsDto(
                        InputRate: "input.rate",
                        OutputRate: "output.rate",
                        Backlog: "backlog",
                        LatencyP95: "latency.p95",
                        Errors: "errors",
                        Retries: "retries",
                        Status: "status"))
            ],
            Flows:
            [
                new ProcessFlowDto(
                    Id: "branch-1",
                    From: "parent",
                    To: "sink",
                    Kind: ProcessFlowKind.Branch,
                    Label: "Has items",
                    Condition: "items > 0",
                    MetricsPrefix: "flow.branch")
            ]);

        var json = JsonSerializer.Serialize(process, JsonOptions);

        Assert.Contains("\"processId\":\"process-1\"", json);
        Assert.Contains("\"childProcessEndpoint\":\"/monitoring/processes/child\"", json);
        Assert.Contains("\"capabilityKey\":\"child-process\"", json);
        Assert.Contains("\"kind\":\"Branch\"", json);
        Assert.Contains("\"condition\":\"items \\u003E 0\"", json);
    }

    [Fact]
    public void Metrics_contracts_serialize_exact_task_contract_shape()
    {
        var snapshot = new MetricSnapshotDto(
            TimestampUtc: DateTimeOffset.UnixEpoch,
            ApplicationId: "app-1",
            Metrics:
            [
                new MetricValueDto(
                    Key: "events.completed",
                    Label: "Completed",
                    Kind: MetricKind.Counter,
                    Value: 42,
                    Unit: "events",
                    ComponentKey: "reader")
            ]);

        var delta = new MetricDeltaDto(
            MessageType: "metrics.delta",
            TimestampUtc: DateTimeOffset.UnixEpoch.AddSeconds(30),
            ApplicationId: "app-1",
            Sequence: 7,
            Metrics:
            [
                new MetricDeltaValueDto(
                    Key: "events.completed",
                    Value: 4)
            ]);

        var snapshotJson = JsonSerializer.Serialize(snapshot, JsonOptions);
        var deltaJson = JsonSerializer.Serialize(delta, JsonOptions);

        Assert.Contains("\"timestampUtc\":\"1970-01-01T00:00:00+00:00\"", snapshotJson);
        Assert.Contains("\"metrics\":[", snapshotJson);
        Assert.Contains("\"label\":\"Completed\"", snapshotJson);
        Assert.Contains("\"componentKey\":\"reader\"", snapshotJson);
        Assert.Contains("\"messageType\":\"metrics.delta\"", deltaJson);
        Assert.Contains("\"sequence\":7", deltaJson);
        Assert.Contains("\"metrics\":[{\"key\":\"events.completed\",\"value\":4}]", deltaJson);
        Assert.DoesNotContain("instanceId", deltaJson);
        Assert.DoesNotContain("fromTimestamp", deltaJson);
        Assert.DoesNotContain("ratePerSecond", deltaJson);
    }

    [Fact]
    public void Health_contracts_serialize_exact_task_contract_shape()
    {
        var snapshot = new HealthSnapshotDto(
            Status: MonitoringHealthStatus.Degraded,
            Checks:
            [
                new HealthCheckDto(
                    Name: "disk",
                    Status: MonitoringHealthStatus.Degraded,
                    Description: "Low disk")
            ]);

        var changed = new HealthChangedDto(
            MessageType: "health.changed",
            TimestampUtc: DateTimeOffset.UnixEpoch,
            ApplicationId: "app-1",
            OverallStatus: MonitoringHealthStatus.Degraded,
            Changed:
            [
                new HealthCheckDto(
                    Name: "disk",
                    Status: MonitoringHealthStatus.Degraded,
                    Description: "Low disk")
            ]);

        var snapshotJson = JsonSerializer.Serialize(snapshot, JsonOptions);
        var changedJson = JsonSerializer.Serialize(changed, JsonOptions);

        Assert.Contains("\"status\":\"Degraded\"", snapshotJson);
        Assert.Contains("\"name\":\"disk\"", snapshotJson);
        Assert.Contains("\"description\":\"Low disk\"", snapshotJson);
        Assert.Contains("\"messageType\":\"health.changed\"", changedJson);
        Assert.Contains("\"overallStatus\":\"Degraded\"", changedJson);
        Assert.Contains("\"changed\":[", changedJson);
        Assert.DoesNotContain("previousStatus", changedJson);
        Assert.DoesNotContain("currentStatus", changedJson);
        Assert.DoesNotContain("displayName", changedJson);
    }

    [Fact]
    public void Registry_contracts_serialize_exact_task_contract_shape()
    {
        var registration = new MonitoringAppRegistrationDto(
            ApplicationInstanceId: "app-instance-1",
            ApplicationName: "Event Reader",
            ApplicationType: "worker",
            InstanceId: "instance-1",
            Environment: "dev",
            Version: "1.0.0",
            MonitoringBaseUrl: "http://localhost:5000/monitoring",
            Labels: new Dictionary<string, string> { ["region"] = "local" });

        var heartbeat = new MonitoringAppHeartbeatDto(
            ApplicationInstanceId: "app-instance-1",
            Status: "Healthy",
            Version: "1.0.0");

        var list = new MonitoringAppRegistryListDto(
            TimestampUtc: DateTimeOffset.UnixEpoch,
            Apps:
            [
                new MonitoringAppRegistryEntryDto(
                    ApplicationInstanceId: "app-instance-1",
                    ApplicationName: "Event Reader",
                    ApplicationType: "worker",
                    InstanceId: "instance-1",
                    Environment: "dev",
                    Version: "1.0.0",
                    MonitoringBaseUrl: "http://localhost:5000/monitoring",
                    Status: "Healthy",
                    RegisteredAtUtc: DateTimeOffset.UnixEpoch,
                    LastSeenUtc: DateTimeOffset.UnixEpoch.AddSeconds(5),
                    ExpiresAtUtc: DateTimeOffset.UnixEpoch.AddSeconds(60),
                    Labels: new Dictionary<string, string> { ["region"] = "local" })
            ]);

        var registrationJson = JsonSerializer.Serialize(registration, JsonOptions);
        var heartbeatJson = JsonSerializer.Serialize(heartbeat, JsonOptions);
        var listJson = JsonSerializer.Serialize(list, JsonOptions);

        Assert.Contains("\"applicationInstanceId\":\"app-instance-1\"", registrationJson);
        Assert.Contains("\"monitoringBaseUrl\":\"http://localhost:5000/monitoring\"", registrationJson);
        Assert.Contains("\"labels\":{\"region\":\"local\"}", registrationJson);
        Assert.Contains("\"status\":\"Healthy\"", heartbeatJson);
        Assert.Contains("\"timestampUtc\":\"1970-01-01T00:00:00+00:00\"", listJson);
        Assert.Contains("\"apps\":[", listJson);
        Assert.Contains("\"expiresAtUtc\":\"1970-01-01T00:01:00+00:00\"", listJson);
        Assert.DoesNotContain("manifestEndpoint", listJson);
        Assert.DoesNotContain("streamEndpoint", listJson);
        Assert.DoesNotContain("entries", listJson);
    }

    [Fact]
    public void EventReaderFasterStoreStatsDto_supports_named_initialization_and_serializes_exact_spec_fields()
    {
        var stats = new EventReaderFasterStoreStatsDto
        {
            TimestampUtc = DateTimeOffset.UnixEpoch,
            BacklogByState = new Dictionary<string, long> { ["pending"] = 7 },
            BacklogByShard = new Dictionary<int, long> { [1] = 5 },
            ActiveRuntimeModelVersions = [1001, 1002],
            OldestUnfinishedAgeSeconds = 12.5,
            LogPath = "C:/stores/main",
            CheckpointPath = "C:/stores/checkpoints",
            LogDirectoryBytes = 100,
            CheckpointDirectoryBytes = 20,
            DiskFreeBytes = 1_000,
            DiskTotalBytes = 2_000,
            EnqueuedTotal = 11,
            EnqueuedRatePerSecond = 1.1,
            ShardLeasedTotal = 12,
            ShardLeaseRatePerSecond = 1.2,
            OutputLeasedTotal = 13,
            OutputLeaseRatePerSecond = 1.3,
            CompletedTotal = 14,
            CompletedRatePerSecond = 1.4,
            RetryTotal = 15,
            RetryRatePerSecond = 1.5,
            FailedTotal = 16,
            FailedRatePerSecond = 1.6,
            SuppressedTotal = 17,
            SuppressedRatePerSecond = 1.7,
            CheckpointCount = 18,
            LastCheckpointUtc = DateTimeOffset.UnixEpoch.AddSeconds(30),
            LastCheckpointDurationMs = 2.1,
            AverageEnqueueLatencyMs = 3.1,
            P95EnqueueLatencyMs = 4.1,
            AverageShardLeaseLatencyMs = 5.1,
            P95ShardLeaseLatencyMs = 6.1,
            AverageOutputLeaseLatencyMs = 7.1,
            P95OutputLeaseLatencyMs = 8.1,
            CleanupExpiredTotal = 19,
            StaleLeaseRecoveryTotal = 20
        };

        var json = JsonSerializer.Serialize(stats, JsonOptions);

        Assert.Contains("\"timestampUtc\":\"1970-01-01T00:00:00+00:00\"", json);
        Assert.Contains("\"backlogByState\":{\"pending\":7}", json);
        Assert.Contains("\"backlogByShard\":{\"1\":5}", json);
        Assert.Contains("\"activeRuntimeModelVersions\":[1001,1002]", json);
        Assert.Contains("\"oldestUnfinishedAgeSeconds\":12.5", json);
        Assert.Contains("\"shardLeasedTotal\":12", json);
        Assert.Contains("\"outputLeasedTotal\":13", json);
        Assert.Contains("\"p95OutputLeaseLatencyMs\":8.1", json);
        Assert.Contains("\"cleanupExpiredTotal\":19", json);
        Assert.DoesNotContain("storePath", json);
        Assert.DoesNotContain("activeModelVersions", json);
        Assert.DoesNotContain("staleRecoveryRatePerSecond", json);
    }
}
