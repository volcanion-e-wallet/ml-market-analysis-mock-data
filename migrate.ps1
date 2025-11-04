# Migration script
param(
    [Parameter(Mandatory=$true)]
    [string]$MigrationName
)

Write-Host "Creating migration: $MigrationName" -ForegroundColor Green

# Create migration
dotnet ef migrations add $MigrationName `
    --project src/MarketAnalysis.Infrastructure `
    --startup-project src/MarketAnalysis.API `
    --context WriteDbContext

if ($LASTEXITCODE -ne 0) {
    Write-Host "Migration creation failed!" -ForegroundColor Red
    exit 1
}

Write-Host "`nApplying migration to database..." -ForegroundColor Yellow

# Apply migration
dotnet ef database update `
    --project src/MarketAnalysis.Infrastructure `
    --startup-project src/MarketAnalysis.API `
    --context WriteDbContext

if ($LASTEXITCODE -ne 0) {
    Write-Host "Migration application failed!" -ForegroundColor Red
    exit 1
}

Write-Host "`nMigration completed successfully!" -ForegroundColor Green
