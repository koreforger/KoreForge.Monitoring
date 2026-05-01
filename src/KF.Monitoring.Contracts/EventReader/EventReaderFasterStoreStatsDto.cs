namespace KF.Monitoring.Contracts.EventReader;

public sealed record EventReaderFasterStoreStatsDto
{
    public required DateTimeOffset Timestamp { get; init; }

    public required IReadOnlyDictionary<string, long> BacklogByShard { get; init; }

    public required IReadOnlyDictionary<string, long> BacklogByModel { get; init; }

    public required IReadOnlyDictionary<string, string> ActiveModelVersions { get; init; }

    public required double OldestBacklogAgeSeconds { get; init; }

    public required string StorePath { get; init; }

    public required string CheckpointPath { get; init; }

    public required string LeasePath { get; init; }

    public required long StoreDirectorySizeBytes { get; init; }

    public required long CheckpointDirectorySizeBytes { get; init; }

    public required long LeaseDirectorySizeBytes { get; init; }

    public required long AvailableDiskBytes { get; init; }

    public required long EnqueuedTotal { get; init; }

    public required double EnqueuedRatePerSecond { get; init; }

    public required long ShardLeaseTotal { get; init; }

    public required double ShardLeaseRatePerSecond { get; init; }

    public required long OutputLeaseTotal { get; init; }

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

    public required DateTimeOffset? LastCheckpointTime { get; init; }

    public required double LastCheckpointDurationMs { get; init; }

    public required double AverageProcessingLatencyMs { get; init; }

    public required double P95ProcessingLatencyMs { get; init; }

    public required double AverageQueueLatencyMs { get; init; }

    public required double P95QueueLatencyMs { get; init; }

    public required long CleanupTotal { get; init; }

    public required double CleanupRatePerSecond { get; init; }

    public required DateTimeOffset? LastCleanupTime { get; init; }

    public required long StaleRecoveryTotal { get; init; }

    public required double StaleRecoveryRatePerSecond { get; init; }

    public required DateTimeOffset? LastStaleRecoveryTime { get; init; }
}
