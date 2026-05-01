using System.Text.Json.Serialization;

namespace KF.Monitoring.Contracts.Metrics;

public sealed record MetricSnapshotDto(
    DateTimeOffset TimestampUtc,
    string ApplicationId,
    IReadOnlyList<MetricValueDto> Metrics);

public sealed record MetricValueDto(
    string Key,
    string Label,
    MetricKind Kind,
    double Value,
    string Unit,
    string ComponentKey);

public sealed record MetricDeltaDto(
    string MessageType,
    DateTimeOffset TimestampUtc,
    string ApplicationId,
    long Sequence,
    IReadOnlyList<MetricDeltaValueDto> Metrics);

public sealed record MetricDeltaValueDto(
    string Key,
    double Value);

[JsonConverter(typeof(JsonStringEnumConverter<MetricKind>))]
public enum MetricKind
{
    Counter,
    Gauge,
    Rate,
    HistogramPercentile
}
