# ── Build Stage ───────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Restore
COPY ["src/AppetitChef.API/AppetitChef.API.csproj", "src/AppetitChef.API/"]
COPY ["src/AppetitChef.Application/AppetitChef.Application.csproj", "src/AppetitChef.Application/"]
COPY ["src/AppetitChef.Domain/AppetitChef.Domain.csproj", "src/AppetitChef.Domain/"]
COPY ["src/AppetitChef.Infrastructure/AppetitChef.Infrastructure.csproj", "src/AppetitChef.Infrastructure/"]
RUN dotnet restore "src/AppetitChef.API/AppetitChef.API.csproj"

# Build
COPY . .
WORKDIR "/src/src/AppetitChef.API"
RUN dotnet build "AppetitChef.API.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "AppetitChef.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ── Runtime Stage ─────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Security: non-root user
RUN addgroup --system appgroup && adduser --system --ingroup appgroup appuser
USER appuser

COPY --from=publish /app/publish .

EXPOSE 8080
EXPOSE 8081

ENTRYPOINT ["dotnet", "AppetitChef.API.dll"]
