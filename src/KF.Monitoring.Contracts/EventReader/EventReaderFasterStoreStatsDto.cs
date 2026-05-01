namespace KF.Monitoring.Contracts.EventReader;

public sealed record EventReaderFasterStoreStatsDto
{
    public required DateTimeOffset TimestampUtc { get; init; }

    public required IReadOnlyDictionary<string, long> BacklogByState { get; init; }

    public required IReadOnlyDictionary<int, long> BacklogByShard { get; init; }

    public required IReadOnlyList<long> ActiveRuntimeModelVersions { get; init; }

    public required double? OldestUnfinishedAgeSeconds { get; init; }

    public required string LogPath { get; init; }

    public required string CheckpointPath { get; init; }

    public required long LogDirectoryBytes { get; init; }

    public required long CheckpointDirectoryBytes { get; init; }

    public required long DiskFreeBytes { get; init; }

    public required long DiskTotalBytes { get; init; }

    public required long EnqueuedTotal { get; init; }

    public required double EnqueuedRatePerSecond { get; init; }

    public required long ShardLeasedTotal { get; init; }

    public required double ShardLeaseRatePerSecond { get; init; }

    public required long OutputLeasedTotal { get; init; }

    public required double OutputLeaseRatePerSecond { get; init; }

    public required long CompletedTotal { get; init; }

    public required double CompletedRatePerSecond { get; init; }

    public required long RetryTotal { get; init; }

    public required double RetryRatePerSecond { get; init; }

    public required long FailedTotal { get; init; }

    public required double FailedRatePerSecond { get; init; }

    public required long SuppressedTotal { get; init; }

    public required double SuppressedRatePerSecond { get; init; }

    public required long CheckpointCount { get; init; }

    public required DateTimeOffset? LastCheckpointUtc { get; init; }

    public required double LastCheckpointDurationMs { get; init; }

    public required double AverageEnqueueLatencyMs { get; init; }

    public required double P95EnqueueLatencyMs { get; init; }

    public required double AverageShardLeaseLatencyMs { get; init; }

    public required double P95ShardLeaseLatencyMs { get; init; }

    public required double AverageOutputLeaseLatencyMs { get; init; }

    public required double P95OutputLeaseLatencyMs { get; init; }

    public required long CleanupExpiredTotal { get; init; }

    public required long StaleLeaseRecoveryTotal { get; init; }
}
