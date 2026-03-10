# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Restore dependencies
COPY GameStore.Api/GameStore.Api.csproj GameStore.Api/
RUN dotnet restore GameStore.Api/GameStore.Api.csproj

# Build and publish
COPY GameStore.Api/ GameStore.Api/
WORKDIR /src/GameStore.Api
RUN dotnet publish -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Create directory for the SQLite database
RUN mkdir -p /app/data

COPY --from=build /app/publish .

# Render injects PORT at runtime; fall back to 8080 locally
CMD ["sh", "-c", "dotnet GameStore.Api.dll --urls http://0.0.0.0:${PORT:-8080}"]
