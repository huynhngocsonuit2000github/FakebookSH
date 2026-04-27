# Model

Development

- Use Docker Compose
- Run databases, message broker, microservices, BFF, maybe UI
- Code locally
- Connect local code to containers
- Store endpoints in environment/appsettings files

Testing

- Deploy to Kubernetes
- Expose only UI and BFF
- Keep all microservices private inside the cluster
- use ExternalName Service for external storage

## Example BFF config

```json
// dev
{
  "Services": {
    "AuthService": "http://localhost:5001",
    "UserService": "http://localhost:5002",
    "PostService": "http://localhost:5003",
    "FeedService": "http://localhost:5004",
    "SocialService": "http://localhost:5005",
    "MediaService": "http://localhost:5006"
  }
}

// test
{
  "Services": {
    "AuthService": "http://auth-service",
    "UserService": "http://user-service",
    "PostService": "http://post-service",
    "FeedService": "http://feed-service",
    "SocialService": "http://social-service",
    "MediaService": "http://media-service"
  }
}
```

# 🚀 Fakebook Backend Progress Tracker

## ✅ Phase 1 – Core (MVP)

### BFF

- [x] Signup API
- [x] Login API
- [x] Logout API
- [x] Get current user (`/me`)
- [ ] Home feed API

### Auth Service

- [x] Register
- [x] Login
- [x] Refresh token
- [x] Logout
- [x] Get current user

### User Service

- [ ] Get current user profile
- [ ] Get user by id
- [ ] Get user by username
- [ ] Update profile

---

## 📰 Phase 2 – Feed & Posts

### Post Service

- [ ] Create post
- [ ] Get post by id
- [ ] Update post
- [ ] Delete post
- [ ] Get posts by user

### Feed Service

- [ ] Get home feed
- [ ] Get user feed

---

## 👥 Phase 3 – Social

### Social Graph Service

- [ ] Follow user
- [ ] Unfollow user
- [ ] Get followers
- [ ] Get following
- [ ] Suggested users

---

## 📸 Phase 4 – Media

### Media Service

- [ ] Upload media
- [ ] Get media
- [ ] Delete media

---

## ❤️ Phase 5 – Engagement

### Engagement Service

- [ ] Like post
- [ ] Unlike post
- [ ] Get reactions
- [ ] Create comment
- [ ] Get comments
- [ ] Delete comment

---

## 🔔 Phase 6 – Notifications

### Notification Service

- [ ] Get notifications
- [ ] Get unread count
- [ ] Mark as read
- [ ] Mark all as read

---

## 💬 Phase 7 – (Optional Later)

### Messaging Service

- [ ] Create conversation
- [ ] Get conversations
- [ ] Send message
- [ ] Get messages

### Search Service

- [ ] Search users
- [ ] Search posts
- [ ] Trending topics

### Story Service

- [ ] Create story
- [ ] Get stories feed
- [ ] Delete story

---

## 🎯 Current Goal

- [ ] User can sign up
- [ ] User can log in
- [ ] User can open feed
- [ ] User can create post

# Additional but require

- Docker
- CI/CD
- Helm
- TLS
- Message broker and async events
- Observability: Add monitoring, logging, and tracing. Cluster model
  - Kubernetes Cluster
    ├── Your Apps (UI, BFF, microservices)
    ├── Observability Stack
    │ ├── Prometheus
    │ ├── Grafana
    │ ├── Loki
    │ ├── Tempo
- Security
  - [ ] HTTPS with TLS
  - [ ] Kubernetes Secrets
  - [ ] External Secrets Operator
  - [ ] Image vulnerability scanning
  - [ ] Dependency scanning
  - [ ] Secret scanning
  - [ ] NetworkPolicy
  - [ ] RBAC
  - [ ] ServiceAccount per service
  - [ ] Non-root containers
  - [ ] CORS config
  - [ ] Rate limiting on BFF

- API Gateway / BFF improvements
  - [ ] Authentication middleware
  - [ ] Request validation
  - [ ] Rate limiting
  - [ ] CORS policy
  - [ ] Error handling middleware
  - [ ] Correlation ID
  - [ ] Request logging
  - [ ] Timeout when calling services
  - [ ] Retry for safe calls
  - [ ] Circuit breaker later

## Env setting

- Dev env
  - [ ] Angular local
  - [ ] BFF local or container
  - [ ] Microservices in Docker Compose
  - [ ] PostgreSQL container
  - [ ] Redis container
  - [ ] RabbitMQ container
  - [ ] MinIO container for local S3-like storage
  - [ ] MailHog for email testing

## Structure

```
fakebook/
  apps/
    ui/
    bff/

  services/
    auth-service/
    user-service/
    post-service/
    feed-service/
    social-service/
    media-service/
    engagement-service/
    notification-service/

  packages/
    shared-contracts/
    shared-logging/
    shared-observability/

  deploy/
    docker/
      docker-compose.yml
      docker-compose.dev.yml

    helm/
      fakebook-ui/
      fakebook-bff/
      fakebook-auth-service/
      fakebook-user-service/
      fakebook-post-service/

    gitops/
      apps/
      environments/
        test/
        staging/

  infra/
    terraform/
      modules/
      envs/
        test/
        staging/

  observability/
    grafana/
      dashboards/
    prometheus/
      alerts/

  docs/
    architecture.md
    local-development.md
    deployment.md
    runbook.md
    troubleshooting.md

  .github/
    workflows/
      ci.yml
      docker-build.yml
      security-scan.yml
      deploy-test.yml
```
