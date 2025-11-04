# Market Analysis Mock Data - Real-time Stock Market Simulation

Ứng dụng C# .NET 8 Web API mô phỏng dữ liệu thị trường chứng khoán theo thời gian thực với kiến trúc DDD, CQRS và các design patterns hiện đại.

## � New! Improved Simulation Architecture (v2.0)

**Simulation Job đã được tách riêng thành Console App độc lập!**

- ✅ **Tách biệt concerns**: Simulation chạy riêng, không ảnh hưởng API
- ✅ **Scalable**: Có thể chạy nhiều simulation jobs song song
- ✅ **Full validation**: Dữ liệu đi qua API validation pipeline
- ✅ **Easy management**: Restart/stop độc lập

📖 **Chi tiết:** Xem [SIMULATION-ARCHITECTURE.md](SIMULATION-ARCHITECTURE.md)

---

## �🏗️ Kiến trúc

- **Domain-Driven Design (DDD)**: Tổ chức code theo các domain nghiệp vụ
- **CQRS Pattern**: Tách biệt Command (ghi) và Query (đọc) với database độc lập
- **Mediator Pattern**: Sử dụng MediatR để quản lý requests/responses
- **Repository Pattern**: Abstraction layer cho data access
- **Producer/Consumer Pattern**: Simulation Job → API → Database

## 🛠️ Công nghệ

- **.NET 8**: Framework chính
- **PostgreSQL Cluster**: 3 nodes với read/write separation
- **Redis**: Distributed caching
- **Elasticsearch**: Centralized logging
- **Prometheus**: Metrics và monitoring
- **Docker & Docker Compose**: Container orchestration
- **GitHub Actions**: CI/CD pipeline
- **Dependabot**: Dependency management
- **Serilog**: Structured logging

## 📁 Cấu trúc dự án

```
src/
├── MarketAnalysis.Domain/          # Domain layer (Entities, Value Objects, Aggregates)
├── MarketAnalysis.Application/     # Application layer (CQRS, Handlers, DTOs)
├── MarketAnalysis.Infrastructure/  # Infrastructure (Database, Redis, Logging)
├── MarketAnalysis.API/            # Presentation layer (Controllers, Middleware)
└── MarketAnalysis.SimulationJob/  # 🆕 Market simulation console app

tests/
├── MarketAnalysis.Domain.Tests/
├── MarketAnalysis.Application.Tests/
└── MarketAnalysis.IntegrationTests/
```

## 🚀 Chạy ứng dụng

### Prerequisites
- Docker & Docker Compose
- .NET 8 SDK (cho development)

### Option 1: Docker Compose (Recommended)

```bash
# Khởi động toàn bộ stack (API + Simulation + Infrastructure)
docker-compose up -d

# Xem logs
docker logs -f market-analysis-api
docker logs -f market-simulation-job
```

### Option 2: Local Development

```powershell
# Quick start script (starts API + Simulation)
.\quick-start.ps1

# Or manually:
# Terminal 1: API
cd src\MarketAnalysis.API
dotnet run

# Terminal 2: Simulation
.\run-simulation.ps1
```

Services sẽ khả dụng tại:
- **API**: http://localhost:5000
- **Swagger UI**: http://localhost:5000/swagger
- **Prometheus**: http://localhost:9090
- **Grafana**: http://localhost:3000 (admin/admin123)
- **Kibana**: http://localhost:5601
- **Elasticsearch**: http://localhost:9200

## 📊 API Endpoints

### Stocks Management
- `GET /api/stocks` - Lấy danh sách cổ phiếu
- `GET /api/stocks/{symbol}` - Lấy thông tin chi tiết cổ phiếu
- `POST /api/stocks` - Tạo cổ phiếu mới
- `PUT /api/stocks/{symbol}/price` - Cập nhật giá cổ phiếu

### Market Data (Realtime)
- `GET /api/market/realtime/{symbol}` - Lấy market tick mới nhất
- `GET /api/market/statistics` - Thống kê thị trường
- `GET /api/market/health` - Health check

### 🆕 Market Ticks (Producer Endpoints)
- `POST /api/market/ticks` - Tạo market tick đơn lẻ
- `POST /api/market/ticks/batch` - Tạo nhiều ticks (batch mode)

## 🔧 Development

### Build & Test

```bash
# Build toàn bộ solution
dotnet build

# Chạy tests
dotnet test

# Chạy với coverage
dotnet test /p:CollectCoverage=true
```

### Chạy migrations

```bash
dotnet ef migrations add InitialCreate --project src/MarketAnalysis.Infrastructure --startup-project src/MarketAnalysis.API
dotnet ef database update --project src/MarketAnalysis.Infrastructure --startup-project src/MarketAnalysis.API
```

---

## 📚 Documentation

- [SIMULATION-ARCHITECTURE.md](SIMULATION-ARCHITECTURE.md) - Chi tiết kiến trúc simulation mới
- [IMPROVEMENT-SUMMARY.md](IMPROVEMENT-SUMMARY.md) - Tổng hợp các cải tiến
- [QUICKSTART.md](QUICKSTART.md) - Hướng dẫn bắt đầu nhanh
- [FIX-SUMMARY.md](FIX-SUMMARY.md) - Lịch sử fix bugs

---

## 🎯 Sample Data

Simulation tự động tạo 10 cổ phiếu mẫu:

| Symbol | Name | Sector | Initial Price |
|--------|------|--------|---------------|
| AAPL | Apple Inc. | Technology | $175.50 |
| GOOGL | Alphabet Inc. | Technology | $140.25 |
| MSFT | Microsoft Corp. | Technology | $380.75 |
| AMZN | Amazon.com Inc. | Consumer Cyclical | $145.80 |
| TSLA | Tesla Inc. | Consumer Cyclical | $245.30 |
| META | Meta Platforms Inc. | Technology | $485.60 |
| NVDA | NVIDIA Corp. | Technology | $495.25 |
| JPM | JPMorgan Chase & Co. | Financial | $185.40 |
| V | Visa Inc. | Financial | $265.90 |
| WMT | Walmart Inc. | Consumer Defensive | $165.75 |

---

## ⚙️ Configuration

### Simulation Settings

File: `src/MarketAnalysis.SimulationJob/appsettings.json`

```json
{
  "SimulationSettings": {
    "IntervalSeconds": 5,
    "EnableBatchMode": true,
    "MinPriceChangePercent": -2.0,
    "MaxPriceChangePercent": 2.0
  },
  "ApiSettings": {
    "BaseUrl": "http://localhost:5000"
  }
}
```

### Docker Environment Variables

```yaml
simulation-job:
  environment:
    - ApiSettings__BaseUrl=http://api:8080
    - SimulationSettings__IntervalSeconds=5
```

---

## 🐳 Docker Services

| Service | Port | Description |
|---------|------|-------------|
| api | 5000 | Main Web API |
| simulation-job | - | Market data generator |
| postgres-master | 5432 | PostgreSQL write node |
| postgres-replica1 | 5433 | PostgreSQL read node 1 |
| postgres-replica2 | 5434 | PostgreSQL read node 2 |
| redis | 6379 | Distributed cache |
| elasticsearch | 9200 | Centralized logging |
| kibana | 5601 | Log visualization |
| prometheus | 9090 | Metrics collection |
| grafana | 3000 | Metrics dashboard |

---

## 📈 Monitoring

### Prometheus Metrics
```
# API metrics
http://localhost:9090/graph
```

### Grafana Dashboards
```
http://localhost:3000
Login: admin/admin123
```

### Elasticsearch Logs
```
http://localhost:5601
```

---

## 🧪 Testing

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test tests/MarketAnalysis.Domain.Tests

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"
```

**Current Test Status:** ✅ 6/6 tests passing

---

## 🚢 CI/CD

GitHub Actions workflows:
- **Build & Test**: On every push/PR
- **Code Quality**: SonarCloud analysis
- **Docker Build**: Multi-stage builds
- **Dependabot**: Auto dependency updates

---

## 📦 NuGet Packages

**Core:**
- Microsoft.EntityFrameworkCore 8.0.10
- Npgsql.EntityFrameworkCore.PostgreSQL 8.0.10
- MediatR 12.4.1
- AutoMapper 13.0.1
- FluentValidation 11.9.2

**Infrastructure:**
- StackExchange.Redis 2.8.16
- Serilog 4.1.0
- prometheus-net.AspNetCore 8.2.1

**Testing:**
- xUnit 2.9.2
- FluentAssertions 6.12.2
- Moq 4.20.72

---

## 🤝 Contributing

1. Fork the project
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 📝 License

This project is for educational purposes.

---

## 🎉 Status

**Build:** ✅ Success  
**Tests:** ✅ 6/6 passing  
**Docker:** ✅ Ready  
**Simulation:** ✅ Refactored & Improved  
**Documentation:** ✅ Complete

**Version:** 2.0 (Improved Architecture)  
**Last Updated:** 2024-01-04

## 📝 License

MIT
