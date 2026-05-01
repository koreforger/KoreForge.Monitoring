using System.Text.Json;
using KF.Monitoring.Contracts.EventReader;
using KF.Monitoring.Contracts.Health;
using KF.Monitoring.Contracts.Manifest;
using KF.Monitoring.Contracts.Metrics;
using KF.Monitoring.Contracts.Process;
using Xunit;

namespace KF.Monitoring.Contracts.Tests;

public sealed class ContractSerializationTests
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    [Fact]
    public void MonitoringManifestDto_serializes_with_web_camel_case_names()
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
            Process: null,
            MetricsSnapshot: null,
            Stream: "/monitoring/stream",
            Health: new HealthSnapshotDto(
                Status: MonitoringHealthStatus.Healthy,
                Checks: [],
                Timestamp: DateTimeOffset.UnixEpoch));

        var json = JsonSerializer.Serialize(manifest, JsonOptions);

        Assert.Contains("\"applicationId\":\"app-1\"", json);
        Assert.Contains("\"monitoringContractVersion\":\"1\"", json);
        Assert.Contains("\"displayName\":\"Event Reader\"", json);
        Assert.Contains("\"metricsSnapshot\":null", json);
        Assert.Contains("\"stream\":\"/monitoring/stream\"", json);
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
    public void MetricDeltaDto_serializes_delta_values_with_web_camel_case_names()
    {
        var delta = new MetricDeltaDto(
            ApplicationId: "app-1",
            InstanceId: "instance-1",
            FromTimestamp: DateTimeOffset.UnixEpoch,
            ToTimestamp: DateTimeOffset.UnixEpoch.AddSeconds(30),
            Values:
            [
                new MetricDeltaValueDto(
                    Key: "events.completed",
                    Kind: MetricKind.Counter,
                    Delta: 42,
                    RatePerSecond: 1.4,
                    Unit: "events")
            ]);

        var json = JsonSerializer.Serialize(delta, JsonOptions);

        Assert.Contains("\"applicationId\":\"app-1\"", json);
        Assert.Contains("\"fromTimestamp\":\"1970-01-01T00:00:00+00:00\"", json);
        Assert.Contains("\"ratePerSecond\":1.4", json);
        Assert.Contains("\"kind\":\"Counter\"", json);
    }

    [Fact]
    public void EventReaderFasterStoreStatsDto_supports_named_initialization_and_serializes_spec_fields()
    {
        var stats = new EventReaderFasterStoreStatsDto
        {
            Timestamp = DateTimeOffset.UnixEpoch,
            BacklogByShard = new Dictionary<string, long> { ["shard-1"] = 7 },
            BacklogByModel = new Dictionary<string, long> { ["model-a"] = 5 },
            ActiveModelVersions = new Dictionary<string, string> { ["projection-a"] = "v2" },
            OldestBacklogAgeSeconds = 12.5,
            StorePath = "C:/stores/main",
            CheckpointPath = "C:/stores/checkpoints",
            LeasePath = "C:/stores/leases",
            StoreDirectorySizeBytes = 100,
            CheckpointDirectorySizeBytes = 20,
            LeaseDirectorySizeBytes = 10,
            AvailableDiskBytes = 1_000,
            EnqueuedTotal = 11,
            EnqueuedRatePerSecond = 1.1,
            ShardLeaseTotal = 12,
            ShardLeaseRatePerSecond = 1.2,
            OutputLeaseTotal = 13,
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
            LastCheckpointTime = DateTimeOffset.UnixEpoch.AddSeconds(30),
            LastCheckpointDurationMs = 2.1,
            AverageProcessingLatencyMs = 3.1,
            P95ProcessingLatencyMs = 4.1,
            AverageQueueLatencyMs = 5.1,
            P95QueueLatencyMs = 6.1,
            CleanupTotal = 19,
            CleanupRatePerSecond = 1.9,
            LastCleanupTime = DateTimeOffset.UnixEpoch.AddSeconds(60),
            StaleRecoveryTotal = 20,
            StaleRecoveryRatePerSecond = 2.0,
            LastStaleRecoveryTime = DateTimeOffset.UnixEpoch.AddSeconds(90)
        };

        var json = JsonSerializer.Serialize(stats, JsonOptions);

        Assert.Contains("\"timestamp\":\"1970-01-01T00:00:00+00:00\"", json);
        Assert.Contains("\"backlogByShard\":{\"shard-1\":7}", json);
        Assert.Contains("\"activeModelVersions\":{\"projection-a\":\"v2\"}", json);
        Assert.Contains("\"oldestBacklogAgeSeconds\":12.5", json);
        Assert.Contains("\"completedRatePerSecond\":1.4", json);
        Assert.Contains("\"p95ProcessingLatencyMs\":4.1", json);
        Assert.Contains("\"staleRecoveryRatePerSecond\":2", json);
    }
}
