# KoreForge.Monitoring

KoreForge.Monitoring contains the shared contracts and runtime pieces for the KoreForge browser-based monitoring shell.

The first package, `KoreForge.Monitoring.Contracts`, defines the DTOs exchanged between monitored applications, the in-memory registry service, and the Vue monitoring shell. It is intended for applications that want to expose monitoring metadata, process diagrams, metric snapshots, SignalR stream payloads, health state, and application-specific panels.

## When To Use It

Use this package when a KoreForge application needs to participate in the Monitoring Shell protocol.

For V1, the package is mainly useful with the companion Monitoring components that will be added in this repository:

- `KoreForge.Monitoring.Registry` for the basic in-memory heartbeat registry.
- `KoreForge.Monitoring.AspNetCore` for exposing monitoring endpoints and SignalR streams from ASP.NET Core applications.
- The standalone Vue shell hosted separately, for example behind nginx.

The contracts can also be used directly by tests, adapters, or application-specific providers such as EventReader's FASTER durable cache panel.

## Scope

V1 is read-only. It supports discovery, heartbeat registration, process definitions, metrics, health, SignalR live updates, and application-specific read-only panels. Operational actions, authentication hardening, Kubernetes-native discovery, and runtime-loaded Vue plugins are planned for later versions.

## EventReader

EventReader is the first client application targeted by this package. Its FASTER durable cache statistics are represented by `EventReaderFasterStoreStatsDto`, allowing the shell to show cache size, backlog, lease rates, completion rates, checkpoint data, latency, expiry cleanup, and stale lease recovery as part of the process drill-down experience.
