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
                      ┌─────┴──────┐
                      │ PostgreSQL │
                      │   :57500   │
                      │  (Docker)  │
                      │            │
                      │ • Database │
                      │ • Aspire   │
                      │   Managed  │
                      └────────────┘
```

### 🐳 Docker Compose Architecture

```
┌─────────────────────────────────────────────────────────┐
│                    Nginx Load Balancer                 │
│                   http://localhost                     │
│              (Reverse Proxy & API Load Balancing)      │
└────────────────────────┬────────────────────────────────┘
                         │
        ┌────────────────┼─────────────────┼────────────────┐
        │                │                 │                │
        │         ┌──────┴──────┐  ┌───────┴──────┐  ┌──────┴──────┐
        │         │   React UI  │  │  Blazor UI   │  │  API Load   │
        │         │   (Proxied) │  │   (Proxied)  │  │  Balancing  │
        │         │             │  │              │  │             │
        │         │ • SPA App   │  │ • SPA App    │  │ • Round     │
        │         │ • Nginx     │  │ • Nginx      │  │   Robin     │
        │         │   Served    │  │   Served     │  │ • Health    │
        │         │ • Route: /  │  │ • Route:     │  │   Checks    │
        │         │             │  │   /blazor/   │  │             │
        │         └─────────────┘  └──────────────┘  └──────┬──────┘
        │                                 │
        │           ┌─────────────────────┼─────────────────────┐
        │           │                     │                     │
        │   ┌───────┴──────┐      ┌───────┴──────┐      ┌───────┴──────┐
        │   │  API Node 1  │      │  API Node 2  │      │ ... API N    │
        │   │ :5001 (HTTP) │      │ :5002 (HTTP) │      │ :500N (HTTP) │
        │   │              │      │              │      │              │
        │   │ • REST API   │      │ • REST API   │      │ • REST API   │
        │   │ • OpenAPI    │      │ • OpenAPI    │      │ • OpenAPI    │
        │   │ • Health     │      │ • Health     │      │ • Health     │
        │   │   Endpoints  │      │   Endpoints  │      │   Endpoints  │
        │   └───────┬──────┘      └───────┬──────┘      └───────┬──────┘
        │           └─────────────────────┼─────────────────────┘
        │                                 │
        │                           ┌─────┴──────┐
        │                           │PostgreSQL  │
        │                           │ :5432      │
        │                           │ (Docker)   │
        │                           │            │
        │                           │ • Database │
        │                           │ • Persist  │
        │                           │   Volume   │
        │                           └─────┬──────┘
        │                                 │
┌───────┴──────┐        ┌─────────────────┼─────────────────┐
│ Blazor UI    │        │                 │                 │
│ :5003 (HTTP) │    ┌───┴──────┐  ┌───────┴──────┐  ┌───────┴──────┐
│              │    │k6 Tests  │  │  ZAP Scanner │  │  NBomber     │
│ • Static     │    │(On-demand│  │  (On-demand) │  │  (On-demand) │
│   Assets     │    │          │  │              │  │              │
│ • Nginx      │    │• Load    │  │ • Security   │  │ • Load       │
│   Served     │    │  Testing │  │   Testing    │  │   Testing    │
│ • Direct     │    │• JS      │  │ • OWASP      │  │ • C#/.NET    │
│   Access     │    │  Scripts │  │   ZAP Proxy  │  │   Framework  │
│ • /blazor/   │    └──────────┘  └──────────────┘  └──────────────┘
│   via nginx  │
└──────────────┘
```

## 🚀 Quick Start

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) for Windows
- Git (to clone the repository)

### 🎯 **Recommended: .NET Aspire Orchestration**

**✅ Includes:** Aspire Dashboard, Service Discovery, Health Monitoring, Telemetry
**🌐 Dashboard:** https://localhost:17182

1. **Start the complete application with Aspire**:
   ```powershell
   cd Testers.AppHost
   dotnet run
   ```

2. **Access all services:**

| Service | URL | Description |
|---------|-----|-------------|
| **🎛️ Aspire Dashboard** | [https://localhost:17182](https://localhost:17182) | Service orchestration & monitoring |
| **🔌 API Service** | Dynamic port (see dashboard) | REST API with health checks |
| **📋 OpenAPI Spec** | Dynamic port (see dashboard) | API documentation (OpenAPI 3.1.1) |
| **🔷 Blazor Frontend** | Dynamic port (see dashboard) | Blazor WebAssembly interface |
| **⚛️ React Frontend** | Dynamic port (see dashboard) | React-based interface |
| **🗄️ PostgreSQL** | Dynamic port (see dashboard) | Database (auto-configured) |

### 🐳 **Alternative: Docker Compose**

**✅ Includes:** Nginx Load Balancer, Multi-API instances, Direct Port Access
**❌ No:** Aspire Dashboard, Service Discovery, Built-in Telemetry

For Docker-only testing without Aspire:
```powershell
docker-compose up --build
```

**⚠️ Note**: The Aspire Dashboard is **NOT** available when using Docker Compose. It only works with the .NET Aspire orchestration above.

#### Docker Compose Service URLs
- **Nginx Load Balancer**: http://localhost
- **React Frontend**: http://localhost/ (via nginx proxy)
- **Blazor Frontend**: http://localhost/blazor/ (via nginx proxy) or http://localhost:5003 (direct)
- **API via Load Balancer**: http://localhost/api/
- **API Instance 1**: http://localhost:5001
- **API Instance 2**: http://localhost:5002
- **PostgreSQL**: localhost:5432

## � **Step-by-Step Startup Procedure**

### **Option 1: .NET Aspire Orchestration (Recommended)**

Follow these steps to get the complete environment running with the Aspire Dashboard:

#### **1. Prerequisites Check**
```powershell
# Verify .NET 9 SDK is installed
dotnet --version
# Should show 9.x.x

# Verify Docker Desktop is running
docker --version
# Should show Docker version info

# Trust development certificates (one-time setup)
dotnet dev-certs https --trust
# Click "Yes" when prompted
```

#### **2. Clone and Navigate to Project**
```powershell
# Clone the repository (if not already done)
git clone <repository-url>
cd AB.TestersPlayground
```

#### **3. Start Aspire Orchestration**
```powershell
# Navigate to the Aspire host project
cd Testers.AppHost

# Start all services with Aspire
dotnet run
```

#### **4. Access the Dashboard**
1. **Wait for startup** - Look for this message:
   ```
   Login to the dashboard at https://localhost:17182/login?t=<token>
   ```

2. **Open Aspire Dashboard**: Navigate to [https://localhost:17182](https://localhost:17182)
   - You may see a certificate warning - click "Advanced" → "Proceed to localhost"

3. **Verify Services**: In the dashboard, you should see:
   - ✅ **api** - .NET API service
   - ✅ **frontend-blazor** - Blazor WebAssembly frontend
   - ✅ **frontend-react** - React frontend  
   - ✅ **postgres** - PostgreSQL database

#### **5. Test the Services**
```powershell
# Test API (replace port with actual from dashboard)
Invoke-RestMethod -Uri "https://localhost:<api-port>/test"

# Test OpenAPI spec
Invoke-RestMethod -Uri "https://localhost:<api-port>/openapi/v1.json"
```

### **Option 2: Docker Compose (Load Balancer Setup)**

Follow these steps for the Docker Compose setup with nginx load balancer:

#### **1. Prerequisites Check**
```powershell
# Verify Docker Desktop is running
docker --version
docker-compose --version

# Stop any running Aspire services to avoid port conflicts
Get-Process -Name "dotnet" | Stop-Process -Force
```

#### **2. Start Docker Compose Services**
```powershell
# From the project root directory
cd AB.TestersPlayground

# Build and start all services
docker-compose up --build
```

#### **3. Wait for Services to Start**
Watch the logs for these success messages:
```
✅ abtestersplayground-api1-1         | Application started. Press Ctrl+C to shut down.
✅ abtestersplayground-api2-1         | Application started. Press Ctrl+C to shut down.
✅ abtestersplayground-nginx-1        | nginx: [notice] start worker processes
✅ abtestersplayground-frontend-*     | ready - started server on 0.0.0.0:80
```

#### **4. Verify All Services**
```powershell
# Check all containers are running
docker-compose ps
# Should show all services as "Up"

# Test the load balancer
Invoke-RestMethod -Uri "http://localhost/api/test"

# Test direct API instances
Invoke-RestMethod -Uri "http://localhost:5001/test"
Invoke-RestMethod -Uri "http://localhost:5002/test"

# Test frontends
Invoke-WebRequest -Uri "http://localhost/" -Method Head      # React
Invoke-WebRequest -Uri "http://localhost/blazor/" -Method Head  # Blazor
```

#### **5. Access the Applications**
- **React Frontend**: http://localhost/
- **Blazor Frontend**: http://localhost/blazor/ or http://localhost:5003
- **API Load Balancer**: http://localhost/api/
- **Direct API Access**: http://localhost:5001, http://localhost:5002
- **PostgreSQL**: localhost:5432 (user: nftuser, password: nftpass, db: nftshop)

### **🛑 Stopping Services**

#### **Stop Aspire:**
```powershell
# In the terminal running Aspire, press Ctrl+C
# Or force stop all .NET processes:
Get-Process -Name "dotnet" | Stop-Process -Force
```

#### **Stop Docker Compose:**
```powershell
# Stop and remove containers
docker-compose down

# Stop and remove containers + volumes (clean slate)
docker-compose down -v
```

### **🔧 Troubleshooting Startup Issues**

#### **Port Conflicts:**
```powershell
# Check what's using specific ports
netstat -ano | findstr ":5001"
netstat -ano | findstr ":17182"

# Kill conflicting processes
Get-Process -Name "dotnet" | Stop-Process -Force
docker-compose down
```

#### **Certificate Issues:**
```powershell
# Re-trust certificates
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```

#### **Docker Build Failures:**
```powershell
# Clean Docker environment
docker system prune -f
docker-compose down -v
docker-compose up --build --force-recreate
```

#### **Aspire Build Errors:**
```powershell
# Clean and rebuild
cd Testers.AppHost
dotnet clean
dotnet build
dotnet run
```

## �📁 Project Structure

```
AB.TestersPlayground/
├── Testers.AppHost/             # 🎛️ .NET Aspire orchestration
│   ├── Program.cs              # Service configuration & startup
│   └── Testers.AppHost.csproj  # Aspire hosting project
├── Testers.ServiceDefaults/     # 🔧 Shared service configurations
│   ├── Extensions.cs           # Health checks, telemetry, etc.
│   └── *.csproj               # Service defaults package
├── api/                         # 🔌 .NET 9 Web API backend
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
- `GET /openapi/v1.json` - OpenAPI 3.1.1 specification (.NET 9 built-in)
- **Interactive Viewing**: Use online Swagger viewers or browser extensions to view the OpenAPI spec

### Testing the API
```powershell
# Via Load Balancer (Docker Compose)
# Health check
Invoke-RestMethod -Uri "http://localhost/api/test"

# Weather data  
Invoke-RestMethod -Uri "http://localhost/api/weatherforecast"

# Service information
Invoke-RestMethod -Uri "http://localhost/api/info"

# OpenAPI specification
Invoke-RestMethod -Uri "http://localhost:5001/openapi/v1.json"

# Direct API Access (Docker Compose)
# API Instance 1
Invoke-RestMethod -Uri "http://localhost:5001/test"

# API Instance 2
Invoke-RestMethod -Uri "http://localhost:5002/test"
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
- .NET 9 SDK
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
- .NET 9 Web API with OpenAPI/Swagger
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

### Deployment Method Comparison

| Feature | .NET Aspire Orchestration | Docker Compose |
|---------|---------------------------|----------------|
| **🎛️ Aspire Dashboard** | ✅ https://localhost:17182 | ❌ Not Available |
| **🔍 Service Discovery** | ✅ Automatic | ❌ Manual Configuration |
| **📊 Built-in Telemetry** | ✅ OTEL/Metrics | ❌ Manual Setup |
| **🏥 Health Monitoring** | ✅ Centralized Dashboard | ❌ Manual Checks |
| **🔀 Load Balancing** | ❌ Single API Instance | ✅ Nginx + Multi API |
| **🐳 Container Management** | ✅ Integrated | ✅ Docker Compose |
| **⚡ Quick Development** | ✅ `dotnet run` | ✅ `docker-compose up` |

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

**✅ SOLUTION**: The Aspire Dashboard is now working! Here's what was fixed:

1. **Fixed Dockerfile paths**: Updated `frontend-blazor/Dockerfile` to use correct relative paths when built from Aspire context
2. **Trusted certificates**: Ran `dotnet dev-certs https --trust` to resolve SSL issues
3. **Stopped port conflicts**: Ensured Docker Compose services were stopped before starting Aspire

**🎉 Current Status**: 
- ✅ Aspire Dashboard: https://localhost:17182 (Working!)
- ✅ API Service: Dynamic port via Aspire (Working!)
- ✅ All services visible in dashboard with health monitoring

**Aspire Dashboard Not Loading:**
```powershell
# The Aspire Dashboard is ONLY available with .NET Aspire orchestration
# NOT with Docker Compose. Use this command:
cd Testers.AppHost
dotnet run

# Then visit: https://localhost:17182
# You may need to accept the self-signed certificate warning in your browser
```

**Docker Issues:**
```powershell
# Reset Docker environment
docker system prune -f
docker-compose down -v
```

## 📚 Additional Resources

- [.NET Aspire Documentation](https://learn.microsoft.com/en-us/dotnet/aspire/)
- [.NET 9 Documentation](https://learn.microsoft.com/en-us/dotnet/)
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

## 🛠️ Tech Stack

### **Core Technologies**
- **📦 .NET 9.0** - Latest stable .NET framework
- **🎛️ .NET Aspire** - Cloud-native orchestration and service management
- **🐳 Docker & Docker Compose** - Containerization and local development
- **🗄️ PostgreSQL** - Primary database (Aspire-managed)

### **Backend Services**
- **🔌 ASP.NET Core Web API** - RESTful backend services
- **📋 OpenAPI/Swagger** - API documentation and testing
- **🔧 Service Defaults** - Shared configurations for health checks, telemetry, and logging
- **📊 Telemetry & Monitoring** - Built-in observability with Aspire

### **Frontend Technologies**
- **🔷 Blazor WebAssembly** - .NET-based SPA framework
- **⚛️ React 18+** - Modern JavaScript UI library
- **🌐 Nginx** - Static file serving and reverse proxy
- **📱 Responsive Design** - Mobile-first UI approach

### **Testing & Quality Assurance**
- **⚡ k6** - JavaScript-based load testing
- **🏋️ NBomber** - C#/.NET performance testing framework
- **🔒 OWASP ZAP** - Security vulnerability scanning
- **🩺 Health Checks** - Service availability monitoring
- **📈 Load Balancing** - Nginx-based traffic distribution

### **Development & DevOps**
- **🔄 Multi-stage Docker builds** - Optimized container images
- **🎯 Hot reload** - Fast development iteration
- **📦 NuGet packages** - Dependency management
- **📝 HTTP files** - API testing and documentation
- **🐙 Container orchestration** - Docker Compose for local development

### **Architecture Patterns**
- **🏗️ Microservices** - Loosely coupled service architecture
- **🔌 Service discovery** - Aspire-managed service communication
- **🛡️ Health-first design** - Comprehensive health checking
- **📊 Observability** - Distributed tracing and metrics
- **🔧 Configuration management** - Environment-based settings
