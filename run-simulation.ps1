# Run Simulation Job Locally

Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "Market Simulation Job Runner" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""

$simulationProject = "src\MarketAnalysis.SimulationJob\MarketAnalysis.SimulationJob.csproj"

if (-not (Test-Path $simulationProject)) {
    Write-Host "[ERROR] Simulation project not found: $simulationProject" -ForegroundColor Red
    exit 1
}

Write-Host "[INFO] Checking if API is running..." -ForegroundColor Yellow

try {
    $response = Invoke-WebRequest -Uri "http://localhost:5000/health" -Method Get -TimeoutSec 5 -UseBasicParsing
    if ($response.StatusCode -eq 200) {
        Write-Host "[SUCCESS] API is healthy and ready" -ForegroundColor Green
    }
} catch {
    Write-Host "[WARNING] API is not running. Please start the API first:" -ForegroundColor Yellow
    Write-Host "  cd src\MarketAnalysis.API" -ForegroundColor Gray
    Write-Host "  dotnet run" -ForegroundColor Gray
    Write-Host ""
    $continue = Read-Host "Continue anyway? (y/n)"
    if ($continue -ne "y") {
        exit 0
    }
}

Write-Host ""
Write-Host "[INFO] Building Simulation Job..." -ForegroundColor Yellow
dotnet build $simulationProject

if ($LASTEXITCODE -ne 0) {
    Write-Host "[ERROR] Build failed" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "[SUCCESS] Build completed" -ForegroundColor Green
Write-Host ""
Write-Host "[INFO] Starting Simulation Job..." -ForegroundColor Cyan
Write-Host "[INFO] Press Ctrl+C to stop" -ForegroundColor Gray
Write-Host ""

# Set environment for Development
$env:DOTNET_ENVIRONMENT = "Development"
$env:ApiSettings__BaseUrl = "http://localhost:5000"
$env:SimulationSettings__IntervalSeconds = "3"
$env:SimulationSettings__EnableBatchMode = "true"

dotnet run --project $simulationProject --no-build
