# Next step after this begin version - Phase 1

After this works locally, add features in this order:

1. Move `AuthService` from Infrastructure into Application using repository interfaces. [done]
2. Add request validation. [done]
3. Add correlation ID middleware + correlation delegating handler. [done]
4. Add centralized exception handling. [done]
5. Add Dockerfile for the API. [done]
6. HTTPS for serice [done]
7. Add message broker events such as `UserRegistered`.
   1. Message publisher [done]
   2. Message consumer [done]

# Microservice advance for senior - Phase 2

8. idempotency for consumers to avoid consumed twice
9. Add retry + dead-letter behavior for messaging
10. Add Outbox pattern
11. Add structured logging

```
CorrelationId
MessageId
UserId
ServiceName
Environment
RequestPath
StatusCode
ElapsedMilliseconds
```

12. Centralize log

```md
- Local/Docker env
  Serilog → Console + Seq

- Kubernetes local/dev cluster (Grafana + Loki + Fluent Bit)
  Serilog → Console
  Kubernetes → Fluent Bit
  Logs → Grafana Loki
  Dashboard → Grafana

- Cloud environment
  Azure:
  Serilog → Console → Azure Monitor / Application Insights
```
