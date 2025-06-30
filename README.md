# 🧪 Testers Playground

A comprehensive multi-service testing playground built with .NET Aspire orchestration, designed to help developers and testers improve their automation testing skills. This project simulates a real-world microservices architecture with multiple frontend technologies, APIs, databases, and testing tools.

## 🏗️ Architecture

```
┌─────────────────────────────────────────────────────────┐
│                 Aspire Dashboard                        │
│              https://localhost:17182                    │
│          (Service Management & Monitoring)              │
└─────────────────────────────────────────────────────────┘
                            │
          ┌─────────────────┼─────────────────┐
          │                 │                 │
┌─────────▼──────┐ ┌────────▼────────┐ ┌─────▼──────┐
│   API Service  │ │ Blazor Frontend │ │   React    │
│ :7122 (HTTPS)  │ │   :5003 (HTTP)  │ │:3000 (HTTP)│
│                │ │                 │ │            │
│ • REST API     │ │ • Static HTML   │ │ • SPA App  │
│ • Swagger UI   │ │ • API Testing   │ │ • Nginx    │
│ • ServiceDefaults│ │ • Nginx Served │ │            │
│ • Health Checks│ │                 │ │            │
│ • Telemetry    │ │                 │ │            │
└─────────┬──────┘ └─────────────────┘ └────────────┘
          │
    ┌─────▼──────┐
    │ PostgreSQL │
    │   :57500   │
    │  (Docker)  │
    │            │
    │ • Database │
    │ • Aspire   │
    │   Managed  │
    └────────────┘
```

## 🚀 Quick Start

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) (Preview)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) for Windows
- Git (to clone the repository)

### 🎯 **Recommended: .NET Aspire Orchestration**

1. **Start the complete application with Aspire**:
   ```powershell
   cd Testers.AppHost
   dotnet run
   ```

2. **Access all services:**

| Service | URL | Description |
|---------|-----|-------------|
| **�️ Aspire Dashboard** | [https://localhost:17182](https://localhost:17182) | Service orchestration & monitoring |
| **🔌 API Service** | [https://localhost:7122](https://localhost:7122) | REST API with health checks |
| **📋 Swagger UI** | [https://localhost:7122/swagger](https://localhost:7122/swagger) | API documentation & testing |
| **🔷 Blazor Frontend** | [http://localhost:5003](http://localhost:5003) | Blazor WebAssembly interface |
| **⚛️ React Frontend** | [http://localhost:3000](http://localhost:3000) | React-based interface |
| **🗄️ PostgreSQL** | `localhost:57500` | Database (auto-configured) |

### 🐳 **Alternative: Docker Compose**

For Docker-only testing without Aspire:
```powershell
docker-compose up --build
```

## 📁 Project Structure

```
AB.TestersPlayground/
├── Testers.AppHost/             # 🎛️ .NET Aspire orchestration
│   ├── Program.cs              # Service configuration & startup
│   └── Testers.AppHost.csproj  # Aspire hosting project
├── Testers.ServiceDefaults/     # 🔧 Shared service configurations
│   ├── Extensions.cs           # Health checks, telemetry, etc.
│   └── *.csproj               # Service defaults package
├── api/                         # 🔌 .NET 10 Web API backend
│   ├── Program.cs              # API application with OpenAPI
│   ├── Api.csproj             # Project with Aspire integration
│   ├── Dockerfile             # API container definition
│   └── Api.http               # HTTP request examples
├── frontend-blazor/            # 🔷 Blazor WebAssembly frontend
│   ├── Components/            # Blazor components
│   ├── wwwroot/              # Static web assets
│   ├── Dockerfile            # Blazor container (nginx-served)
│   └── nginx/                # Nginx configuration
├── frontend-react/             # ⚛️ React frontend application
│   ├── src/                  # React source code
│   ├── Dockerfile            # React container definition
│   └── nginx/                # Nginx configuration
├── nginx/                      # 🔀 Load balancer (Docker Compose)
├── tests/                      # 🧪 Testing tools and scripts
│   ├── k6/                   # JavaScript load testing
│   ├── nbomber/              # C# load testing
│   └── zap/                  # Security testing with OWASP ZAP
├── docker-compose.yml          # 🐳 Container orchestration
├── TestersPlayground.sln       # 📦 Visual Studio solution
└── README.md                  # 📖 This documentation
```

## 🔌 API Endpoints

The API service provides the following endpoints:

### Health & Info
- `GET /test` - Simple health check with timestamp
- `GET /info` - Detailed service information
- `GET /health` - Aspire health check endpoint

### Sample Data
- `GET /weatherforecast` - Sample weather data (5-day forecast)

### Documentation
- `GET /openapi/v1.json` - OpenAPI 3.1.1 specification (.NET 10 built-in)
- **Interactive Viewing**: Use online Swagger viewers or browser extensions to view the OpenAPI spec

### Testing the API
```powershell
# Health check
Invoke-RestMethod -Uri "http://localhost:5001/test"

# Weather data  
Invoke-RestMethod -Uri "http://localhost:5001/weatherforecast"

# Service information
Invoke-RestMethod -Uri "http://localhost:5001/info"

# OpenAPI specification
Invoke-RestMethod -Uri "http://localhost:5001/openapi/v1.json"
```

## 🧪 Testing Capabilities

### Load Testing Tools
- **k6** (JavaScript): Modern load testing with scripts in `tests/k6/`
- **NBomber** (C#): Enterprise-grade load testing framework in `tests/nbomber/`

### Security Testing
- **OWASP ZAP**: Automated security vulnerability scanning in `tests/zap/`

### API Testing Scenarios
- RESTful endpoints testing
- Health check validation
- Database connectivity testing
- Service discovery testing
- Telemetry and monitoring

### Frontend Testing
- Multi-technology testing (React & Blazor)
- API integration testing
- Static asset serving
- Cross-origin request testing

## 🔧 Development & Advanced Usage

### Aspire Service Management

**View all services status:**
```powershell
# Open Aspire Dashboard
# Navigate to https://localhost:17182 in your browser
```

**Individual service development:**
```powershell
# API only
cd api
dotnet run

# Run with specific environment
dotnet run --environment Production
```

### Docker Compose Operations
```powershell
# Start specific services
docker-compose up --build api frontend-react db

# View logs
docker-compose logs -f api

# Stop all services
docker-compose down

# Clean up containers and volumes
docker-compose down -v
```

### Local Development Setup

**Prerequisites for local development:**
- .NET 10 SDK
- Node.js 18+ and npm
- PostgreSQL (optional - Aspire can manage this)

**API Development:**
```powershell
cd api
dotnet restore
dotnet run
```

**React Frontend:**
```powershell
cd frontend-react
npm install
npm start
```

**Blazor Frontend:**
```powershell
cd frontend-blazor
dotnet run
```

## 🎯 Learning Objectives

Use this playground to practice:

### 🔄 **Service Orchestration**
- .NET Aspire orchestration patterns
- Service discovery and communication
- Health monitoring and telemetry
- Container orchestration with Docker

### 🧪 **Testing Techniques**
- **API Testing**: REST endpoint validation, OpenAPI testing
- **Load Testing**: Performance under various conditions
- **Security Testing**: Vulnerability assessment
- **Integration Testing**: Multi-service interaction
- **UI Automation**: Frontend testing across frameworks
- **Database Testing**: Data persistence validation

### 🏗️ **Architecture Patterns**
- Microservices architecture
- Frontend/backend separation
- Database integration patterns
- Service mesh concepts
- Health check implementations

## 🌟 Key Features

### ✅ **Aspire Integration**
- Centralized service orchestration
- Built-in health checks and telemetry
- Service discovery and configuration
- Real-time monitoring dashboard

### ✅ **Multi-Technology Stack**
- .NET 10 Web API with OpenAPI/Swagger
- React frontend with modern tooling
- Blazor WebAssembly with static serving
- PostgreSQL database with Docker

### ✅ **Development Tools**
- Interactive API documentation (Swagger)
- Hot-reload development support
- Comprehensive logging and monitoring
- Multiple deployment options

### ✅ **Testing Infrastructure**
- Multiple load testing frameworks
- Security testing tools
- Sample test scripts and configurations
- CI/CD ready structure

## 🛠️ Troubleshooting

### Common Issues

**Port Conflicts:**
```powershell
# Stop conflicting services
docker-compose down
Get-Process -Name "dotnet" | Stop-Process -Force
```

**Certificate Issues:**
```powershell
# Trust development certificates
dotnet dev-certs https --trust
```

**Docker Issues:**
```powershell
# Reset Docker environment
docker system prune -f
docker-compose down -v
```

### Service URLs Reference

#### Docker Compose Services
- Nginx Load Balancer: http://localhost
- API Instance 1: http://localhost:5001  
- API Instance 2: http://localhost:5002
- OpenAPI Spec: http://localhost:5001/openapi/v1.json
- Blazor Frontend: http://localhost:5003
- React Frontend: http://localhost:3000
- PostgreSQL: localhost:5432

#### Aspire Orchestration
- Aspire Dashboard: https://localhost:17182
- API Service: https://localhost:7122
- OpenAPI Spec: https://localhost:7122/openapi/v1.json
- Blazor Frontend: Dynamic port (see dashboard)
- React Frontend: Dynamic port (see dashboard)
- PostgreSQL: Dynamic port (see dashboard)

## 📚 Additional Resources

- [.NET Aspire Documentation](https://learn.microsoft.com/en-us/dotnet/aspire/)
- [.NET 10 Documentation](https://learn.microsoft.com/en-us/dotnet/)
- [Docker Documentation](https://docs.docker.com/)
- [OpenAPI/Swagger Documentation](https://swagger.io/docs/)
- [k6 Load Testing](https://k6.io/docs/)
- [NBomber Documentation](https://nbomber.com/)
- [OWASP ZAP Documentation](https://www.zaproxy.org/docs/)

## 🤝 Contributing

This is an educational project designed for learning and experimentation:

- ✨ Add new testing scenarios and examples
- 🔧 Improve service configurations
- 📝 Share testing best practices and patterns
- 🐛 Report issues and suggest improvements
- 🎓 Create educational content and tutorials

## 📝 License

This project is for educational and testing purposes. Feel free to use, modify, and distribute for learning and development.

---

## 🎉 **Ready to Start Testing!**

**🚀 Quick Start Command:**
```powershell
cd Testers.AppHost && dotnet run
```

**🌐 Then visit:** [https://localhost:17182](https://localhost:17182)

*Happy Testing! 🧪✨ This playground gives you a complete modern microservices environment to practice your testing skills across multiple technologies and scenarios.*
