FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY ["src/MarketAnalysis.SimulationJob/MarketAnalysis.SimulationJob.csproj", "src/MarketAnalysis.SimulationJob/"]
RUN dotnet restore "src/MarketAnalysis.SimulationJob/MarketAnalysis.SimulationJob.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/src/MarketAnalysis.SimulationJob"
RUN dotnet build "MarketAnalysis.SimulationJob.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MarketAnalysis.SimulationJob.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MarketAnalysis.SimulationJob.dll"]
