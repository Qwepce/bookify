# Bookify

Bookify - backend-сервис на .NET для бронирования апартаментов. Проект описывает основной сценарий бронирования: пользователей, апартаменты, брони, отзывы, аутентификацию, хранение данных, кеширование, наблюдаемость и фоновые задачи.

## Структура Проекта

- `src/Bookify.Api` - ASP.NET Core API, контроллеры, версионированные endpoints, Swagger, health checks, логирование и middleware.
- `src/Bookify.Application` - прикладные сценарии, абстракции, валидация, контракты кеширования и обработчики запросов.
- `src/Bookify.Domain` - доменная модель апартаментов, броней, отзывов, пользователей и общих value objects.
- `src/Bookify.Infrastructure` - работа с PostgreSQL, Redis, Keycloak, репозиториями, миграциями, email и outbox-процессингом.
- `tests/Bookify.UnitTests` - unit-тесты доменной и прикладной логики.
- `tests/Bookify.IntegrationTests` - интеграционные тесты API и инфраструктурных сценариев.
- `tests/Bookify.ArchitectureTests` - тесты архитектурных ограничений.

## Технологии

- .NET 10 и ASP.NET Core
- Entity Framework Core и PostgreSQL
- Redis для кеширования
- Keycloak для identity и JWT-аутентификации
- Serilog и Seq для структурированного логирования
- Quartz для фоновых задач
- Docker Compose для локальной инфраструктуры

## Локальный Запуск

Запустите приложение и инфраструктурные сервисы через Docker Compose:

```bash
docker compose up --build
```

Compose-конфигурация поднимает:

- Bookify API на `http://localhost:5001`
- PostgreSQL на `localhost:5432`
- Keycloak на `http://localhost:18080`
- Seq на `http://localhost:8081`
- Redis на `localhost:6379`

В development-режиме API автоматически применяет миграции базы данных и включает Swagger UI для просмотра доступных endpoints.

## Тесты

Запуск всех тестов из корня репозитория:

```bash
dotnet test Bookify.slnx
```
