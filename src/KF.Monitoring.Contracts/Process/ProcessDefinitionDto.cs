using System.Text.Json.Serialization;

namespace KF.Monitoring.Contracts.Process;

public sealed record ProcessDefinitionDto(
    string ProcessId,
    string Name,
    string Version,
    IReadOnlyList<ProcessStepDto> Steps,
    IReadOnlyList<ProcessFlowDto> Flows);

public sealed record ProcessStepDto(
    string Id,
    string Label,
    ProcessStepKind Kind,
    string ComponentKey,
    string MetricsPrefix,
    string? ChildProcessEndpoint = null,
    string? CapabilityKey = null,
    StepMetricBindingsDto? MetricBindings = null);

public sealed record StepMetricBindingsDto(
    string? InputRate = null,
    string? OutputRate = null,
    string? Backlog = null,
    string? LatencyP95 = null,
    string? Errors = null,
    string? Retries = null,
    string? Status = null);

public sealed record ProcessFlowDto(
    string Id,
    string From,
    string To,
    ProcessFlowKind Kind,
    string? Label = null,
    string? Condition = null,
    string? MetricsPrefix = null);

[JsonConverter(typeof(JsonStringEnumConverter<ProcessStepKind>))]
public enum ProcessStepKind
{
    Source,
    Process,
    Queue,
    WorkerPool,
    Publisher,
    Sink
}

[JsonConverter(typeof(JsonStringEnumConverter<ProcessFlowKind>))]
public enum ProcessFlowKind
{
    Normal,
    Branch,
    Duplicate,
    Retry,
    Error,
    Join
}
