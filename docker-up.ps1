# Docker Compose Up script
Write-Host "Starting Market Analysis Stack..." -ForegroundColor Green

# Stop any existing containers
Write-Host "`nStopping existing containers..." -ForegroundColor Yellow
docker-compose down

# Build and start containers
Write-Host "`nBuilding and starting containers..." -ForegroundColor Yellow
docker-compose up -d --build

if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to start containers!" -ForegroundColor Red
    exit 1
}

Write-Host "`nWaiting for services to be healthy..." -ForegroundColor Yellow
Start-Sleep -Seconds 30

# Show status
Write-Host "`nContainer status:" -ForegroundColor Green
docker-compose ps

Write-Host "`n✅ Stack is running!" -ForegroundColor Green
Write-Host "`nServices available at:" -ForegroundColor Cyan
Write-Host "  - API:            http://localhost:5000" -ForegroundColor White
Write-Host "  - Swagger:        http://localhost:5000/swagger" -ForegroundColor White
Write-Host "  - Prometheus:     http://localhost:9090" -ForegroundColor White
Write-Host "  - Grafana:        http://localhost:3000 (admin/admin123)" -ForegroundColor White
Write-Host "  - Kibana:         http://localhost:5601" -ForegroundColor White
Write-Host "  - Elasticsearch:  http://localhost:9200" -ForegroundColor White
Write-Host "`nTo view logs: docker-compose logs -f" -ForegroundColor Yellow
Write-Host "To stop: docker-compose down" -ForegroundColor Yellow
