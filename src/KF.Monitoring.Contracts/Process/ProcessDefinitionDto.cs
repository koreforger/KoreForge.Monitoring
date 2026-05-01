using System.Text.Json.Serialization;

namespace KF.Monitoring.Contracts.Process;

/// <summary>
/// Defines a monitorable process as steps and directed flows between those steps.
/// </summary>
/// <param name="ProcessId">Stable process identifier.</param>
/// <param name="Name">Human-readable process name.</param>
/// <param name="Version">Version of the process definition.</param>
/// <param name="Steps">Steps shown in the process diagram.</param>
/// <param name="Flows">Directed connections between process steps.</param>
public sealed record ProcessDefinitionDto(
    string ProcessId,
    string Name,
    string Version,
    IReadOnlyList<ProcessStepDto> Steps,
    IReadOnlyList<ProcessFlowDto> Flows);

/// <summary>
/// Describes one node in a monitorable process diagram.
/// </summary>
/// <param name="Id">Stable step identifier unique within the process.</param>
/// <param name="Label">Display label for the step.</param>
/// <param name="Kind">Visual and semantic kind of step.</param>
/// <param name="ComponentKey">Component key used by the shell for specialized rendering.</param>
/// <param name="MetricsPrefix">Metric key prefix associated with this step.</param>
/// <param name="ChildProcessEndpoint">Optional endpoint for drilling into a nested process.</param>
/// <param name="CapabilityKey">Optional capability key for a related detail panel.</param>
/// <param name="MetricBindings">Optional metric keys bound to standard step indicators.</param>
public sealed record ProcessStepDto(
    string Id,
    string Label,
    ProcessStepKind Kind,
    string ComponentKey,
    string MetricsPrefix,
    string? ChildProcessEndpoint = null,
    string? CapabilityKey = null,
    StepMetricBindingsDto? MetricBindings = null);

/// <summary>
/// Maps standard process step indicators to metric keys.
/// </summary>
/// <param name="InputRate">Metric key for incoming throughput.</param>
/// <param name="OutputRate">Metric key for outgoing throughput.</param>
/// <param name="Backlog">Metric key for queued or unfinished work.</param>
/// <param name="LatencyP95">Metric key for p95 latency.</param>
/// <param name="Errors">Metric key for errors.</param>
/// <param name="Retries">Metric key for retries.</param>
/// <param name="Status">Metric key for status.</param>
public sealed record StepMetricBindingsDto(
    string? InputRate = null,
    string? OutputRate = null,
    string? Backlog = null,
    string? LatencyP95 = null,
    string? Errors = null,
    string? Retries = null,
    string? Status = null);

/// <summary>
/// Describes a directed connection between two process steps.
/// </summary>
/// <param name="Id">Stable flow identifier unique within the process.</param>
/// <param name="From">Source step identifier.</param>
/// <param name="To">Target step identifier.</param>
/// <param name="Kind">Semantic kind of flow.</param>
/// <param name="Label">Optional display label.</param>
/// <param name="Condition">Optional condition label for branch flows.</param>
/// <param name="MetricsPrefix">Optional metric key prefix associated with the flow.</param>
public sealed record ProcessFlowDto(
    string Id,
    string From,
    string To,
    ProcessFlowKind Kind,
    string? Label = null,
    string? Condition = null,
    string? MetricsPrefix = null);

/// <summary>
/// Classifies the visual and operational role of a process step.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<ProcessStepKind>))]
public enum ProcessStepKind
{
    /// <summary>External source or input stage.</summary>
    Source,
    /// <summary>Processing stage.</summary>
    Process,
    /// <summary>Queue, buffer, or durable store stage.</summary>
    Queue,
    /// <summary>Parallel worker group stage.</summary>
    WorkerPool,
    /// <summary>Publisher or output writer stage.</summary>
    Publisher,
    /// <summary>Terminal sink stage.</summary>
    Sink
}

/// <summary>
/// Classifies the behavior represented by a process flow.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<ProcessFlowKind>))]
public enum ProcessFlowKind
{
    /// <summary>Standard forward flow.</summary>
    Normal,
    /// <summary>Conditional branch flow.</summary>
    Branch,
    /// <summary>Duplicate fan-out flow.</summary>
    Duplicate,
    /// <summary>Retry flow.</summary>
    Retry,
    /// <summary>Error or dead-letter flow.</summary>
    Error,
    /// <summary>Join or fan-in flow.</summary>
    Join
}
