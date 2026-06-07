FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копируем ВСЕ .csproj файлы, а не только Bookify.Api
COPY ["Bookify.Api/Bookify.Api.csproj", "Bookify.Api/"]
COPY ["Bookify.Application/Bookify.Application.csproj", "Bookify.Application/"]
COPY ["Bookify.Infrastructure/Bookify.Infrastructure.csproj", "Bookify.Infrastructure/"]
COPY ["Bookify.Domain/Bookify.Domain.csproj", "Bookify.Domain/"]

# Восстанавливаем зависимости для ВСЕХ проектов
RUN dotnet restore "Bookify.Api/Bookify.Api.csproj"

# Копируем ВЕСЬ исходный код (не только .csproj)
COPY . .

WORKDIR "/src/Bookify.Api"
RUN dotnet build "./Bookify.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Bookify.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bookify.Api.dll"]