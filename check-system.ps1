# System Check Script
Write-Host "=== Market Analysis System Check ===" -ForegroundColor Cyan
Write-Host ""

# 1. Check .NET SDK
Write-Host "Checking .NET SDK..." -ForegroundColor Yellow
$dotnetVersion = dotnet --version
Write-Host ".NET SDK version: $dotnetVersion" -ForegroundColor Green
Write-Host ""

# 2. Check Docker
Write-Host "Checking Docker..." -ForegroundColor Yellow
try {
    $dockerVersion = docker --version
    Write-Host "Docker: $dockerVersion" -ForegroundColor Green
} catch {
    Write-Host "Docker not found or not running" -ForegroundColor Red
}
Write-Host ""

# 3. Check Docker Compose
Write-Host "Checking Docker Compose..." -ForegroundColor Yellow
try {
    $composeVersion = docker-compose --version
    Write-Host "Docker Compose: $composeVersion" -ForegroundColor Green
} catch {
    Write-Host "Docker Compose not found" -ForegroundColor Red
}
Write-Host ""

# 4. Restore packages
Write-Host "Restoring NuGet packages..." -ForegroundColor Yellow
dotnet restore MarketAnalysis.sln --verbosity quiet
if ($LASTEXITCODE -eq 0) {
    Write-Host "Packages restored successfully" -ForegroundColor Green
} else {
    Write-Host "Package restore failed" -ForegroundColor Red
    exit 1
}
Write-Host ""

# 5. Build solution
Write-Host "Building solution..." -ForegroundColor Yellow
dotnet build MarketAnalysis.sln --no-restore --verbosity quiet
if ($LASTEXITCODE -eq 0) {
    Write-Host "Build successful" -ForegroundColor Green
} else {
    Write-Host "Build failed" -ForegroundColor Red
    exit 1
}
Write-Host ""

# 6. Run tests
Write-Host "Running tests..." -ForegroundColor Yellow
dotnet test MarketAnalysis.sln --no-build --verbosity quiet
if ($LASTEXITCODE -eq 0) {
    Write-Host "All tests passed" -ForegroundColor Green
} else {
    Write-Host "Some tests failed" -ForegroundColor Red
    exit 1
}
Write-Host ""

# Summary
Write-Host "=== System Check Summary ===" -ForegroundColor Cyan
Write-Host "All checks passed!" -ForegroundColor Green
Write-Host ""
Write-Host "Ready to deploy! Run '.\docker-up.ps1' to start all services." -ForegroundColor Cyan
