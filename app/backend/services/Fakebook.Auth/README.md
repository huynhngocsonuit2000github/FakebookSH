# Fakebook Auth - Begin Simple

This is a simpler starter version of the Auth service.

It keeps the 4 projects you already created:

```text
Fakebook.Auth.Api
Fakebook.Auth.Application
Fakebook.Auth.Domain
Fakebook.Auth.Infrastructure
```

## What is included

- PostgreSQL in Docker Compose
- EF Core Code First setup
- JWT access token
- Refresh token table
- Register API
- Login API
- Refresh token API
- Logout API
- Get current user API
- Swagger UI in Development

## Project responsibility

```text
Fakebook.Auth.Api
  Controllers and Program.cs

Fakebook.Auth.Application
  DTOs, service interfaces, result wrapper

Fakebook.Auth.Domain
  User and RefreshToken entities

Fakebook.Auth.Infrastructure
  EF Core DbContext, AuthService implementation, JWT, password hashing
```

This is intentionally simpler than a full microservice template. Later, you can split the code more cleanly into repositories, events, observability, outbox, message broker, and Kubernetes files.

## 1. Start PostgreSQL

From this `begin` folder:

```bash
docker compose up -d
```

PostgreSQL connection:

```text
Host: localhost
Port: 5433
Database: fakebook_auth
Username: fakebook
Password: fakebook_password
```

## 2. Restore packages

```bash
dotnet restore
```

## 3. Create the first EF Core migration

Install or update the EF CLI tool if needed:

```bash
dotnet tool update --global dotnet-ef
```

Create migration:

```bash
dotnet ef migrations add InitialCreate --project Fakebook.Auth.Infrastructure --startup-project Fakebook.Auth.Api --context AuthDbContext
```

Apply migration to PostgreSQL:

```bash
dotnet ef database update --project Fakebook.Auth.Infrastructure --startup-project Fakebook.Auth.Api --context AuthDbContext
```

## 4. Run the API

```bash
dotnet run --project Fakebook.Auth.Api --launch-profile https
```

Open Swagger:

```text
https://localhost:7001/swagger
```

## 5. Test flow

### Register

```http
POST /api/auth/register
Content-Type: application/json

{
  "email": "test@example.com",
  "userName": "testuser",
  "password": "Password123!"
}
```

### Login

```http
POST /api/auth/login
Content-Type: application/json

{
  "emailOrUserName": "test@example.com",
  "password": "Password123!"
}
```

Copy the `accessToken` from the response.

### Get current user

```http
GET /api/auth/me
Authorization: Bearer YOUR_ACCESS_TOKEN
```

### Refresh token

```http
POST /api/auth/refresh-token
Content-Type: application/json

{
  "refreshToken": "YOUR_REFRESH_TOKEN"
}
```

### Logout

```http
POST /api/auth/logout
Content-Type: application/json

{
  "refreshToken": "YOUR_REFRESH_TOKEN"
}
```

## Visual Studio notes

If you already have an empty Visual Studio solution with the same 4 projects, you can copy the files from each project folder into your existing project folders.

Then make sure the project references are:

```text
Fakebook.Auth.Api -> Fakebook.Auth.Application
Fakebook.Auth.Api -> Fakebook.Auth.Infrastructure
Fakebook.Auth.Infrastructure -> Fakebook.Auth.Application
Fakebook.Auth.Infrastructure -> Fakebook.Auth.Domain
Fakebook.Auth.Application -> Fakebook.Auth.Domain
```

## Next step after this begin version

After this works locally, add features in this order:

1. Move `AuthService` from Infrastructure into Application using repository interfaces. [done]
2. Add request validation. [done]
3. Add correlation ID middleware + correlation delegating handler. [done]
4. Add centralized exception handling. [done]
5. Add Dockerfile for the API. [done]
6. HTTPS for serice
7. Add message broker events such as `UserRegistered`.
