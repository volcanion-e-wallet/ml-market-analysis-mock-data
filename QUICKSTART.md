# Quick Start Guide

## Prerequisites
- Docker Desktop
- .NET 8 SDK (for local development)
- PowerShell

## Quick Start

### 1. Clone and Navigate to Project
```powershell
cd e:\Github\payment\ml-market-analysis-mock-data
```

### 2. Start All Services with Docker
```powershell
.\docker-up.ps1
```

This will start:
- PostgreSQL cluster (Master + 2 Replicas)
- Redis cache
- Elasticsearch for logging
- Kibana for log visualization
- Prometheus for metrics
- Grafana for dashboards
- Market Analysis API

### 3. Access Services

**API & Documentation:**
- API: http://localhost:5000
- Swagger UI: http://localhost:5000/swagger
- Health Check: http://localhost:5000/health

**Monitoring & Logging:**
- Prometheus: http://localhost:9090
- Grafana: http://localhost:3000 (admin/admin123)
- Kibana: http://localhost:5601
- Elasticsearch: http://localhost:9200

**Databases:**
- PostgreSQL Master (Write): localhost:5432
- PostgreSQL Replica 1 (Read): localhost:5433
- PostgreSQL Replica 2 (Read): localhost:5434
- Redis: localhost:6379

## Development Workflow

### Build the Solution
```powershell
.\build.ps1
```

### Run Tests
```powershell
.\test.ps1
```

### Create and Apply Migration
```powershell
.\migrate.ps1 -MigrationName "YourMigrationName"
```

### View Logs
```powershell
docker-compose logs -f api
docker-compose logs -f postgres-master
```

### Stop All Services
```powershell
docker-compose down
```

### Clean Up Everything (including volumes)
```powershell
docker-compose down -v
```

## API Examples

### Get All Stocks
```bash
curl http://localhost:5000/api/stocks
```

### Get Stock by Symbol
```bash
curl http://localhost:5000/api/stocks/AAPL
```

### Get Real-time Market Data
```bash
curl http://localhost:5000/api/market/realtime/AAPL
```

### Get Market Statistics
```bash
curl http://localhost:5000/api/market/statistics
```

### Create New Stock
```bash
curl -X POST http://localhost:5000/api/stocks \
  -H "Content-Type: application/json" \
  -d '{
    "symbol": "TSLA",
    "name": "Tesla Inc.",
    "exchange": "NASDAQ",
    "sector": "Technology",
    "industry": "Electric Vehicles",
    "initialPrice": 250.00
  }'
```

## Architecture Overview

**DDD Layers:**
- `Domain`: Core business logic, entities, value objects
- `Application`: CQRS commands/queries, DTOs, MediatR handlers
- `Infrastructure`: Database, Redis, Elasticsearch implementations
- `API`: Controllers, middleware, API configuration

**Key Patterns:**
- Domain-Driven Design (DDD)
- CQRS (Command Query Responsibility Segregation)
- Mediator Pattern (via MediatR)
- Repository Pattern
- Dependency Injection

**Technologies:**
- .NET 8 Web API
- PostgreSQL cluster (3 nodes with replication)
- Redis for caching
- Elasticsearch + Kibana for logging
- Prometheus + Grafana for monitoring
- Docker & Docker Compose

## Troubleshooting

### Database Connection Issues
Ensure PostgreSQL containers are healthy:
```powershell
docker-compose ps
```

### API Not Starting
Check API logs:
```powershell
docker-compose logs -f api
```

### Reset Database
```powershell
docker-compose down -v
docker-compose up -d
```

Wait 30 seconds for initialization, then the Market Data Generator will populate sample stocks.

## Project Structure
```
ml-market-analysis-mock-data/
├── src/
│   ├── MarketAnalysis.Domain/       # Domain entities & interfaces
│   ├── MarketAnalysis.Application/  # CQRS commands & queries
│   ├── MarketAnalysis.Infrastructure/ # Data access & services
│   └── MarketAnalysis.API/          # Web API & controllers
├── tests/
│   ├── MarketAnalysis.Domain.Tests/
│   ├── MarketAnalysis.Application.Tests/
│   └── MarketAnalysis.IntegrationTests/
├── docker/                          # Docker configuration files
├── .github/                         # CI/CD workflows
├── docker-compose.yml               # Container orchestration
└── README.md                        # Full documentation
```
