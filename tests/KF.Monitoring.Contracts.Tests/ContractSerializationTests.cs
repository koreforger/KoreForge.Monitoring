using System.Text.Json;
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
}
