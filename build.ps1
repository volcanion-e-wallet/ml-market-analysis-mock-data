# Build script
Write-Host "Building Market Analysis Solution..." -ForegroundColor Green

# Restore packages
Write-Host "`nRestoring NuGet packages..." -ForegroundColor Yellow
dotnet restore MarketAnalysis.sln

if ($LASTEXITCODE -ne 0) {
    Write-Host "Package restore failed!" -ForegroundColor Red
    exit 1
}

# Build solution
Write-Host "`nBuilding solution..." -ForegroundColor Yellow
dotnet build MarketAnalysis.sln --configuration Release --no-restore

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed!" -ForegroundColor Red
    exit 1
}

Write-Host "`nBuild completed successfully!" -ForegroundColor Green
