# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["MarketAnalysis.sln", "./"]
COPY ["src/MarketAnalysis.Domain/MarketAnalysis.Domain.csproj", "src/MarketAnalysis.Domain/"]
COPY ["src/MarketAnalysis.Application/MarketAnalysis.Application.csproj", "src/MarketAnalysis.Application/"]
COPY ["src/MarketAnalysis.Infrastructure/MarketAnalysis.Infrastructure.csproj", "src/MarketAnalysis.Infrastructure/"]
COPY ["src/MarketAnalysis.API/MarketAnalysis.API.csproj", "src/MarketAnalysis.API/"]

# Restore dependencies
RUN dotnet restore "MarketAnalysis.sln"

# Copy all source code
COPY . .

# Build and publish
WORKDIR "/src/src/MarketAnalysis.API"
RUN dotnet build "MarketAnalysis.API.csproj" -c Release -o /app/build
RUN dotnet publish "MarketAnalysis.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Install curl for healthchecks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

EXPOSE 8080
EXPOSE 8081

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "MarketAnalysis.API.dll"]
