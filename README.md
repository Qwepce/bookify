# Bookify

Bookify is a .NET backend for booking apartments. The project models the main booking workflow: users, apartments, bookings, reviews, authentication, persistence, caching, observability, and background processing.

## Project Structure

- `src/Bookify.Api` - ASP.NET Core API host, controllers, versioned endpoints, Swagger, health checks, logging, and middleware.
- `src/Bookify.Application` - application use cases, abstractions, validation, cache contracts, and request handlers.
- `src/Bookify.Domain` - domain model for apartments, bookings, reviews, users, and shared value objects.
- `src/Bookify.Infrastructure` - PostgreSQL persistence, Redis caching, Keycloak authentication, repositories, migrations, email, and outbox processing.
- `tests/Bookify.UnitTests` - unit tests for domain and application behavior.
- `tests/Bookify.IntegrationTests` - integration tests for API and infrastructure scenarios.
- `tests/Bookify.ArchitectureTests` - architecture boundary tests.

## Technology Stack

- .NET 10 and ASP.NET Core
- Entity Framework Core with PostgreSQL
- Redis for caching
- Keycloak for identity and JWT authentication
- Serilog and Seq for structured logging
- Quartz for background jobs
- Docker Compose for local infrastructure

## Local Development

Run the application and supporting services with Docker Compose:

```bash
docker compose up --build
```

The compose setup starts:

- Bookify API on `http://localhost:5001`
- PostgreSQL on `localhost:5432`
- Keycloak on `http://localhost:18080`
- Seq on `http://localhost:8081`
- Redis on `localhost:6379`

In development, the API applies database migrations automatically and exposes Swagger UI for API exploration.

## Tests

Run all tests from the repository root:

```bash
dotnet test Bookify.slnx
```
