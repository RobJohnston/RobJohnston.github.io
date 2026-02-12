---
applyTo: "**/Dockerfile,**/docker-compose.yml"
---

# Docker Instructions

Government of Canada containerized applications using Docker.

## Dockerfile for .NET 8.0

```dockerfile
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["BenefitsApp/BenefitsApp.csproj", "BenefitsApp/"]
RUN dotnet restore "BenefitsApp/BenefitsApp.csproj"

COPY . .
WORKDIR "/src/BenefitsApp"
RUN dotnet build "BenefitsApp.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "BenefitsApp.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Create non-root user (security)
RUN adduser --disabled-password --gecos "" appuser && chown -R appuser:appuser /app
USER appuser

COPY --from=publish /app/publish .

EXPOSE 8080

HEALTHCHECK --interval=30s --timeout=3s CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "BenefitsApp.dll"]
```

## Docker Compose

```yaml
version: '3.8'
services:
  app:
    build: .
    ports:
      - "5000:8080"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__Default=Server=db;Database=benefits;
    depends_on:
      - db

  db:
    image: mcr.microsoft.com/mssql/server:2019-latest
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourStrong!Passw0rd
    volumes:
      - sqldata:/var/opt/mssql

volumes:
  sqldata:
```

## .dockerignore

```
**/bin/
**/obj/
**/node_modules/
**/.git/
**/.vs/
**/*.md
**/Dockerfile*
```

Last updated: 2025-02-11
