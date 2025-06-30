# 🧪 Testers Playground

A comprehensive testing playground designed to help people improve their automation testing skills. This project simulates a real-world microservices architecture with multiple components that can be used to practice various testing techniques.

## 🏗️ Architecture

This playground includes:

- **🔀 Load Balancing**: Nginx distributing requests between API instances
- **🚀 Dual APIs**: Two identical .NET 10 Web API instances for high availability
- **🎨 Multi-Frontend**: Both React and Blazor frontend options
- **💾 Database**: PostgreSQL for data persistence
- **🧪 Testing Suite**: k6 (JavaScript) and OWASP ZAP for comprehensive testing
  - *NBomber (C#) temporarily disabled due to .NET 10 compatibility issues*

## 🚀 Quick Start

### Prerequisites
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) for Windows
- Git (to clone the repository)

### Starting the Application

1. **Open PowerShell** in the project directory
2. **Start the core application** (recommended):
   ```powershell
   docker-compose up --build nginx api1 api2 frontend-react frontend-blazor db
   ```

3. **Access the services:**

| Service | URL | Description |
|---------|-----|-------------|
| **🌐 Main App** | [http://localhost](http://localhost) | Load balancer distributing traffic |
| **⚡ API Instance 1** | [http://localhost:5001](http://localhost:5001) | Direct API access |
| **⚡ API Instance 2** | [http://localhost:5002](http://localhost:5002) | Direct API access |
| **⚛️ React Frontend** | [http://localhost:3000](http://localhost:3000) | React-based interface |
| **🔷 Blazor Frontend** | [http://localhost:5003](http://localhost:5003) | Blazor-based interface |
| **🗄️ Database** | `localhost:5432` | PostgreSQL (user: nftuser, pass: nftpass) |

### Testing the API

Test the weather forecast endpoint:
```powershell
# Via load balancer (recommended)
curl http://localhost/api/weatherforecast

# Direct API access
curl http://localhost:5001/api/weatherforecast
curl http://localhost:5002/api/weatherforecast
```

## 📁 Project Structure

```
AB.TestersPlayground/
├── api/                          # .NET 10 Web API backend
│   ├── Program.cs               # API application entry point
│   ├── Api.csproj              # Project configuration
│   ├── Dockerfile              # API container definition
│   └── Api.http                # HTTP request examples
├── frontend-react/              # React frontend application
│   ├── Dockerfile              # React container definition
│   └── nginx/                  # Nginx configuration for React
├── frontend-blazor/             # Blazor frontend application
│   ├── Dockerfile              # Blazor container definition
│   └── nginx/                  # Nginx configuration for Blazor
├── nginx/                       # Main load balancer configuration
│   └── nginx.conf              # Load balancer settings
├── tests/                       # Testing tools and scripts
│   ├── nbomber/                # C# load testing with NBomber
│   ├── k6/                     # JavaScript load testing with k6
│   └── zap/                    # Security testing with OWASP ZAP
├── docker-compose.yml           # Complete service orchestration
└── README.md                   # This file
```

## 🧪 Testing Capabilities

This playground provides multiple testing scenarios:

### Load Testing
- **k6** (JavaScript): Modern load testing tool
- **NBomber** (C#): Enterprise-grade load testing framework *(temporarily disabled)*

### Security Testing
- **OWASP ZAP**: Automated security vulnerability scanning

### API Testing
- RESTful endpoints for testing HTTP methods
- Load balancer behavior testing
- Database connectivity testing

### UI Testing
- Multiple frontend technologies (React & Blazor)
- Responsive design testing scenarios

## 🔧 Advanced Usage

### Start Specific Services
```powershell
# Only APIs and database
docker-compose up --build api1 api2 db

# Only frontend services
docker-compose up --build frontend-react frontend-blazor nginx

# Everything including testing tools
docker-compose up --build
```

### Stop Services
```powershell
docker-compose down
```

### View Running Containers
```powershell
docker ps
```

## ⚠️ Known Issues

- **NBomber Compatibility**: The NBomber testing service is currently commented out due to compatibility issues with .NET 10 preview. To re-enable it, you would need to update the NBomber packages to support .NET 10 or downgrade to a compatible .NET version.
- **Workaround**: Use k6 for load testing instead, which works perfectly with the current setup.

## 🎯 Learning Objectives

Use this playground to practice:

- **API Testing**: REST endpoint validation, response testing
- **Load Testing**: Performance under various load conditions
- **Security Testing**: Vulnerability assessment and penetration testing
- **Integration Testing**: Multi-service interaction testing
- **UI Automation**: Frontend testing across different frameworks
- **Database Testing**: Data persistence and retrieval validation
- **Container Testing**: Docker-based application testing

## 🛠️ Development

### Local Development
For local development without Docker:

1. **API Development**:
   ```powershell
   cd api
   dotnet run
   ```

2. **React Development**:
   ```powershell
   cd frontend-react
   npm install
   npm start
   ```

### Requirements for Local Development
- .NET 10 SDK
- Node.js and npm
- PostgreSQL (if running database locally)


## 📚 Additional Resources

- [Docker Documentation](https://docs.docker.com/)
- [.NET 10 Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [NBomber Documentation](https://nbomber.com/)
- [k6 Documentation](https://k6.io/docs/)
- [OWASP ZAP Documentation](https://www.zaproxy.org/docs/)

## 🤝 Contributing

This is an educational project. Feel free to:
- Add new testing scenarios
- Improve existing configurations
- Share testing best practices
- Report issues and suggest improvements

## 📝 License

This project is for educational and testing purposes only. No real NFTs are bought or sold.

---

**Happy Testing! 🧪🚀**

*Remember: The best way to learn testing is by doing. Use this playground to experiment, break things, and discover how robust applications should behave under various conditions.*
