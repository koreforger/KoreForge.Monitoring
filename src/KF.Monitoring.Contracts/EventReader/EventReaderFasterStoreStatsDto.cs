namespace KF.Monitoring.Contracts.EventReader;

/// <summary>
/// Captures EventReader FASTER durable cache statistics for monitoring panels and process drill-down.
/// </summary>
public sealed record EventReaderFasterStoreStatsDto
{
    /// <summary>UTC timestamp for the statistics sample.</summary>
    public required DateTimeOffset TimestampUtc { get; init; }

    /// <summary>Current backlog counts grouped by work state.</summary>
    public required IReadOnlyDictionary<string, long> BacklogByState { get; init; }

    /// <summary>Current backlog counts grouped by shard.</summary>
    public required IReadOnlyDictionary<int, long> BacklogByShard { get; init; }

    /// <summary>Runtime model versions currently present in unfinished work.</summary>
    public required IReadOnlyList<long> ActiveRuntimeModelVersions { get; init; }

    /// <summary>Age in seconds of the oldest unfinished record, when available.</summary>
    public required double? OldestUnfinishedAgeSeconds { get; init; }

    /// <summary>Filesystem path for the FASTER log directory.</summary>
    public required string LogPath { get; init; }

    /// <summary>Filesystem path for the FASTER checkpoint directory.</summary>
    public required string CheckpointPath { get; init; }

    /// <summary>Size of the FASTER log directory in bytes.</summary>
    public required long LogDirectoryBytes { get; init; }

    /// <summary>Size of the FASTER checkpoint directory in bytes.</summary>
    public required long CheckpointDirectoryBytes { get; init; }

    /// <summary>Free bytes on the disk containing the durable cache.</summary>
    public required long DiskFreeBytes { get; init; }

    /// <summary>Total bytes on the disk containing the durable cache.</summary>
    public required long DiskTotalBytes { get; init; }

    /// <summary>Total records enqueued into the durable cache.</summary>
    public required long EnqueuedTotal { get; init; }

    /// <summary>Recent enqueue rate in records per second.</summary>
    public required double EnqueuedRatePerSecond { get; init; }

    /// <summary>Total records leased for shard-level processing.</summary>
    public required long ShardLeasedTotal { get; init; }

    /// <summary>Recent shard lease rate in records per second.</summary>
    public required double ShardLeaseRatePerSecond { get; init; }

    /// <summary>Total records leased for output processing.</summary>
    public required long OutputLeasedTotal { get; init; }

    /// <summary>Recent output lease rate in records per second.</summary>
    public required double OutputLeaseRatePerSecond { get; init; }

    /// <summary>Total records completed.</summary>
    public required long CompletedTotal { get; init; }

    /// <summary>Recent completion rate in records per second.</summary>
    public required double CompletedRatePerSecond { get; init; }

    /// <summary>Total records retried.</summary>
    public required long RetryTotal { get; init; }

    /// <summary>Recent retry rate in records per second.</summary>
    public required double RetryRatePerSecond { get; init; }

    /// <summary>Total records failed.</summary>
    public required long FailedTotal { get; init; }

    /// <summary>Recent failure rate in records per second.</summary>
    public required double FailedRatePerSecond { get; init; }

    /// <summary>Total records suppressed by application rules.</summary>
    public required long SuppressedTotal { get; init; }

    /// <summary>Recent suppression rate in records per second.</summary>
    public required double SuppressedRatePerSecond { get; init; }

    /// <summary>Total completed FASTER checkpoint operations.</summary>
    public required long CheckpointCount { get; init; }

    /// <summary>UTC timestamp of the latest completed checkpoint, when available.</summary>
    public required DateTimeOffset? LastCheckpointUtc { get; init; }

    /// <summary>Duration in milliseconds of the latest checkpoint operation.</summary>
    public required double LastCheckpointDurationMs { get; init; }

    /// <summary>Average enqueue latency in milliseconds.</summary>
    public required double AverageEnqueueLatencyMs { get; init; }

    /// <summary>P95 enqueue latency in milliseconds.</summary>
    public required double P95EnqueueLatencyMs { get; init; }

    /// <summary>Average shard lease latency in milliseconds.</summary>
    public required double AverageShardLeaseLatencyMs { get; init; }

    /// <summary>P95 shard lease latency in milliseconds.</summary>
    public required double P95ShardLeaseLatencyMs { get; init; }

    /// <summary>Average output lease latency in milliseconds.</summary>
    public required double AverageOutputLeaseLatencyMs { get; init; }

    /// <summary>P95 output lease latency in milliseconds.</summary>
    public required double P95OutputLeaseLatencyMs { get; init; }

    /// <summary>Total expired records removed by cleanup.</summary>
    public required long CleanupExpiredTotal { get; init; }

    /// <summary>Total stale leases recovered.</summary>
    public required long StaleLeaseRecoveryTotal { get; init; }
}
