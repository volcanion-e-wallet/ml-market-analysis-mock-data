# Run tests script
Write-Host "Running tests..." -ForegroundColor Green

# Run all tests
dotnet test MarketAnalysis.sln --configuration Release --no-build --verbosity normal

if ($LASTEXITCODE -ne 0) {
    Write-Host "Tests failed!" -ForegroundColor Red
    exit 1
}

Write-Host "`nAll tests passed!" -ForegroundColor Green
