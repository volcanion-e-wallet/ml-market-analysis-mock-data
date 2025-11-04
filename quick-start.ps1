# Quick Start - Run API and Simulation Job

Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "Market Analysis - Quick Start" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "[STEP 1] Starting API in background..." -ForegroundColor Yellow
$apiJob = Start-Job -ScriptBlock {
    Set-Location $using:PWD
    dotnet run --project src\MarketAnalysis.API\MarketAnalysis.API.csproj
}

Write-Host "[INFO] Waiting for API to start (30 seconds)..." -ForegroundColor Gray
Start-Sleep -Seconds 30

Write-Host "[STEP 2] Checking API health..." -ForegroundColor Yellow
$healthCheck = $false
for ($i = 1; $i -le 5; $i++) {
    try {
        $response = Invoke-WebRequest -Uri "http://localhost:5000/health" -Method Get -TimeoutSec 5 -UseBasicParsing
        if ($response.StatusCode -eq 200) {
            Write-Host "[SUCCESS] API is running and healthy" -ForegroundColor Green
            $healthCheck = $true
            break
        }
    } catch {
        Write-Host "[RETRY $i/5] API not ready yet..." -ForegroundColor Gray
        Start-Sleep -Seconds 5
    }
}

if (-not $healthCheck) {
    Write-Host "[ERROR] API failed to start" -ForegroundColor Red
    Stop-Job $apiJob
    Remove-Job $apiJob
    exit 1
}

Write-Host ""
Write-Host "[STEP 3] Starting Simulation Job..." -ForegroundColor Yellow
Write-Host ""
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "Both services are now running!" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "API:        http://localhost:5000" -ForegroundColor Cyan
Write-Host "Swagger:    http://localhost:5000/swagger" -ForegroundColor Cyan
Write-Host ""
Write-Host "[INFO] Press Ctrl+C to stop simulation (API will continue running)" -ForegroundColor Gray
Write-Host "[INFO] To stop API, run: Stop-Job -Id $($apiJob.Id)" -ForegroundColor Gray
Write-Host ""

# Set environment for Development
$env:DOTNET_ENVIRONMENT = "Development"
$env:ApiSettings__BaseUrl = "http://localhost:5000"
$env:SimulationSettings__IntervalSeconds = "3"

try {
    dotnet run --project src\MarketAnalysis.SimulationJob\MarketAnalysis.SimulationJob.csproj
} finally {
    Write-Host ""
    Write-Host "[INFO] Simulation stopped. API is still running (Job ID: $($apiJob.Id))" -ForegroundColor Yellow
    Write-Host "[INFO] To stop API: Stop-Job $($apiJob.Id); Remove-Job $($apiJob.Id)" -ForegroundColor Gray
}
