using System.Text.Json.Serialization;

namespace KF.Monitoring.Contracts.Metrics;

public sealed record MetricSnapshotDto(
    string ApplicationId,
    string InstanceId,
    DateTimeOffset Timestamp,
    IReadOnlyList<MetricValueDto> Values);

public sealed record MetricValueDto(
    string Key,
    MetricKind Kind,
    double Value,
    string? Unit = null,
    string? DisplayName = null,
    IReadOnlyDictionary<string, string>? Tags = null);

public sealed record MetricDeltaDto(
    string ApplicationId,
    string InstanceId,
    DateTimeOffset FromTimestamp,
    DateTimeOffset ToTimestamp,
    IReadOnlyList<MetricDeltaValueDto> Values);

public sealed record MetricDeltaValueDto(
    string Key,
    MetricKind Kind,
    double Delta,
    double? RatePerSecond = null,
    string? Unit = null,
    IReadOnlyDictionary<string, string>? Tags = null);

[JsonConverter(typeof(JsonStringEnumConverter<MetricKind>))]
public enum MetricKind
{
    Counter,
    Gauge,
    Rate,
    HistogramPercentile
}
