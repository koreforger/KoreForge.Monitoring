using System.Text.Json.Serialization;

namespace KF.Monitoring.Contracts.Metrics;

/// <summary>
/// Captures the current metric values reported by an application instance.
/// </summary>
/// <param name="TimestampUtc">UTC timestamp for the snapshot.</param>
/// <param name="ApplicationId">Application identifier that produced the snapshot.</param>
/// <param name="Metrics">Metric values included in the snapshot.</param>
public sealed record MetricSnapshotDto(
    DateTimeOffset TimestampUtc,
    string ApplicationId,
    IReadOnlyList<MetricValueDto> Metrics);

/// <summary>
/// Represents a single metric value in a snapshot.
/// </summary>
/// <param name="Key">Stable metric key.</param>
/// <param name="Label">Display label for the metric.</param>
/// <param name="Kind">Metric kind.</param>
/// <param name="Value">Numeric value.</param>
/// <param name="Unit">Display unit.</param>
/// <param name="ComponentKey">Process component associated with the metric.</param>
public sealed record MetricValueDto(
    string Key,
    string Label,
    MetricKind Kind,
    double Value,
    string Unit,
    string ComponentKey);

/// <summary>
/// SignalR payload containing metric changes since a previous sequence.
/// </summary>
/// <param name="MessageType">Message discriminator for the live stream.</param>
/// <param name="TimestampUtc">UTC timestamp for the delta.</param>
/// <param name="ApplicationId">Application identifier that produced the delta.</param>
/// <param name="Sequence">Monotonic sequence number for the stream.</param>
/// <param name="Metrics">Changed metric values.</param>
public sealed record MetricDeltaDto(
    string MessageType,
    DateTimeOffset TimestampUtc,
    string ApplicationId,
    long Sequence,
    IReadOnlyList<MetricDeltaValueDto> Metrics);

/// <summary>
/// Represents one changed metric value in a live delta payload.
/// </summary>
/// <param name="Key">Stable metric key.</param>
/// <param name="Value">Updated numeric value.</param>
public sealed record MetricDeltaValueDto(
    string Key,
    double Value);

/// <summary>
/// Identifies how a metric value should be interpreted and displayed.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<MetricKind>))]
public enum MetricKind
{
    /// <summary>Monotonically increasing count.</summary>
    Counter,
    /// <summary>Point-in-time value.</summary>
    Gauge,
    /// <summary>Value measured per time unit.</summary>
    Rate,
    /// <summary>Histogram percentile value.</summary>
    HistogramPercentile
}
