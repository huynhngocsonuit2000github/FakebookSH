FakebookSH/
│
├── docs/
│ ├── architecture/
│ ├── api/
│ ├── database/
│ ├── deployment/
│ └── decisions/
│
├── frontend/
│ └── fakebook-web/
│ ├── src/
│ │ ├── app/
│ │ │ ├── core/
│ │ │ │ ├── guards/
│ │ │ │ ├── interceptors/
│ │ │ │ ├── services/
│ │ │ │ ├── models/
│ │ │ │ └── constants/
│ │ │ ├── shared/
│ │ │ │ ├── components/
│ │ │ │ ├── directives/
│ │ │ │ ├── pipes/
│ │ │ │ └── ui/
│ │ │ ├── layout/
│ │ │ │ ├── main-layout/
│ │ │ │ └── auth-layout/
│ │ │ ├── features/
│ │ │ │ ├── auth/
│ │ │ │ ├── feed/
│ │ │ │ ├── profile/
│ │ │ │ ├── friends/
│ │ │ │ ├── messages/
│ │ │ │ ├── notifications/
│ │ │ │ ├── settings/
│ │ │ │ └── media/
│ │ │ ├── state/
│ │ │ ├── assets/
│ │ │ ├── environments/
│ │ │ └── app.routes.ts
│ │ ├── styles/
│ │ ├── index.html
│ │ └── main.ts
│ ├── Dockerfile
│ ├── nginx.conf
│ ├── package.json
│ └── angular.json
│
├── bff/
│ └── fakebook-bff/
│ ├── src/
│ │ ├── controllers/
│ │ ├── services/
│ │ ├── clients/
│ │ ├── models/
│ │ ├── middleware/
│ │ ├── config/
│ │ └── Program.cs
│ ├── Dockerfile
│ └── fakebook-bff.csproj
│
├── backend/
│ ├── gateway/
│ │ └── fakebook-api-gateway/
│ │ ├── src/
│ │ ├── Dockerfile
│ │ └── gateway-config/
│ │
│ ├── services/
│ │ ├── auth-service/
│ │ │ ├── src/
│ │ │ │ ├── Fakebook.Auth.Api/
│ │ │ │ ├── Fakebook.Auth.Application/
│ │ │ │ ├── Fakebook.Auth.Domain/
│ │ │ │ ├── Fakebook.Auth.Infrastructure/
│ │ │ │ └── Fakebook.Auth.Contracts/
│ │ │ ├── tests/
│ │ │ └── Dockerfile
│ │ │
│ │ ├── user-service/
│ │ │ ├── src/
│ │ │ │ ├── Fakebook.User.Api/
│ │ │ │ ├── Fakebook.User.Application/
│ │ │ │ ├── Fakebook.User.Domain/
│ │ │ │ ├── Fakebook.User.Infrastructure/
│ │ │ │ └── Fakebook.User.Contracts/
│ │ │ ├── tests/
│ │ │ └── Dockerfile
│ │ │
│ │ ├── post-service/
│ │ │ ├── src/
│ │ │ │ ├── Fakebook.Post.Api/
│ │ │ │ ├── Fakebook.Post.Application/
│ │ │ │ ├── Fakebook.Post.Domain/
│ │ │ │ ├── Fakebook.Post.Infrastructure/
│ │ │ │ └── Fakebook.Post.Contracts/
│ │ │ ├── tests/
│ │ │ └── Dockerfile
│ │ │
│ │ ├── media-service/
│ │ │ ├── src/
│ │ │ ├── tests/
│ │ │ └── Dockerfile
│ │ │
│ │ ├── message-service/
│ │ │ ├── src/
│ │ │ ├── tests/
│ │ │ └── Dockerfile
│ │ │
│ │ └── notification-service/
│ │ ├── src/
│ │ ├── tests/
│ │ └── Dockerfile
│ │
│ ├── building-blocks/
│ │ ├── Fakebook.SharedKernel/
│ │ ├── Fakebook.Common/
│ │ ├── Fakebook.EventBus/
│ │ ├── Fakebook.Observability/
│ │ └── Fakebook.Security/
│ │
│ └── tests/
│ ├── integration/
│ └── contract/
│
├── infrastructure/
│ ├── docker/
│ │ ├── compose/
│ │ │ ├── docker-compose.local.yml
│ │ │ ├── docker-compose.infrastructure.yml
│ │ │ └── .env
│ │ ├── images/
│ │ └── scripts/
│ │
│ ├── kubernetes/
│ │ ├── base/
│ │ │ ├── namespace/
│ │ │ ├── ingress/
│ │ │ ├── configmap/
│ │ │ ├── secrets/
│ │ │ ├── bff/
│ │ │ ├── gateway/
│ │ │ ├── frontend/
│ │ │ ├── auth-service/
│ │ │ ├── user-service/
│ │ │ ├── post-service/
│ │ │ ├── media-service/
│ │ │ ├── message-service/
│ │ │ ├── notification-service/
│ │ │ ├── redis/
│ │ │ ├── rabbitmq/
│ │ │ └── postgres/
│ │ │
│ │ ├── overlays/
│ │ │ ├── local/
│ │ │ ├── dev/
│ │ │ ├── sit/
│ │ │ ├── uat/
│ │ │ └── prod/
│ │ │
│ │ └── helm/
│ │ ├── fakebook-platform/
│ │ └── fakebook-services/
│ │
│ ├── terraform/
│ │ ├── modules/
│ │ ├── environments/
│ │ │ ├── dev/
│ │ │ ├── sit/
│ │ │ ├── uat/
│ │ │ └── prod/
│ │ └── global/
│ │
│ └── monitoring/
│ ├── prometheus/
│ ├── grafana/
│ ├── loki/
│ └── dashboards/
│
├── scripts/
│ ├── setup/
│ ├── build/
│ ├── deploy/
│ ├── migrate/
│ └── dev/
│
├── database/
│ ├── postgres/
│ │ ├── auth-db/
│ │ ├── user-db/
│ │ ├── post-db/
│ │ └── message-db/
│ └── seed/
│
├── .github/
│ └── workflows/
│ ├── frontend-ci.yml
│ ├── bff-ci.yml
│ ├── services-ci.yml
│ ├── docker-build.yml
│ └── deploy.yml
│
├── .editorconfig
├── .gitignore
├── README.md
└── Makefile
