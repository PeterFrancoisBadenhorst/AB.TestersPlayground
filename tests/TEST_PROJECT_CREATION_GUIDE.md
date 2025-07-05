# 🧪 Test Project Creation Guide

This document provides detailed prompts and instructions for creating comprehensive test projects within the AB.TestersPlayground solution. Each test type serves a specific purpose in ensuring the quality and reliability of the application.

## 📁 Test Project Structure Overview

```
tests/
├── unit/                           # Unit Tests
│   ├── Api.UnitTests/             # API unit tests
│   ├── Blazor.UnitTests/          # Blazor component unit tests
│   └── ServiceDefaults.UnitTests/ # Service defaults unit tests
├── integration/                    # Integration Tests
│   ├── Api.IntegrationTests/      # API integration tests
│   ├── Database.IntegrationTests/ # Database integration tests
│   └── Services.IntegrationTests/ # Service-to-service tests
├── functional/                     # Functional/End-to-End Tests
│   ├── WebUI.FunctionalTests/     # UI automation tests
│   ├── API.FunctionalTests/       # API functional tests
│   └── Workflow.FunctionalTests/  # Business workflow tests
├── performance/                    # Performance Tests
│   ├── LoadTests.K6/              # k6 load tests
│   ├── LoadTests.NBomber/         # NBomber performance tests
│   └── StressTests/               # Stress testing
├── security/                       # Security Tests
│   ├── PenetrationTests.ZAP/      # OWASP ZAP security tests
│   ├── ApiSecurity.Tests/         # API security testing
│   └── AuthenticationTests/       # Auth & authorization tests
└── contract/                       # Contract Tests
    ├── ApiContract.Tests/         # API contract testing
    └── DatabaseContract.Tests/    # Database schema tests
```

---

## 🔧 Unit Tests

### Prompt for API Unit Tests

```
Create a comprehensive unit test project for the API layer with the following requirements:

**Project Name**: `Api.UnitTests`
**Target Framework**: .NET 9.0
**Test Framework**: xUnit
**Mocking Framework**: Moq or NSubstitute

**Required NuGet Packages**:
- Microsoft.NET.Test.Sdk
- xunit
- xunit.runner.visualstudio
- Moq (or NSubstitute)
- FluentAssertions
- Microsoft.AspNetCore.Mvc.Testing
- Microsoft.Extensions.Logging.Abstractions

**Test Categories to Include**:
1. **Controller Tests**: Test all API controllers in isolation
   - Test HTTP status codes
   - Test response models
   - Test input validation
   - Test exception handling

2. **Service Layer Tests**: Test business logic services
   - Test data processing logic
   - Test validation rules
   - Test error handling scenarios
   - Test dependency interactions

3. **Model Tests**: Test data models and DTOs
   - Test model validation attributes
   - Test serialization/deserialization
   - Test model mapping

4. **Extension Method Tests**: Test custom extension methods
   - Test utility functions
   - Test configuration extensions

**Folder Structure**:
- Controllers/ (controller tests)
- Services/ (service layer tests)
- Models/ (model tests)
- Extensions/ (extension method tests)
- Helpers/ (test helper classes)

**Test Naming Convention**: Use `MethodName_Scenario_ExpectedResult` pattern

**Additional Requirements**:
- Include test data builders/factories
- Mock external dependencies
- Test both success and failure scenarios
- Include parameterized tests where appropriate
- Maintain high code coverage (>90%)
```

### Prompt for Blazor Component Unit Tests

```
Create a unit test project for Blazor WebAssembly components with the following specifications:

**Project Name**: `Blazor.UnitTests`
**Target Framework**: .NET 9.0
**Test Framework**: bUnit + xUnit

**Required NuGet Packages**:
- Microsoft.NET.Test.Sdk
- bunit
- bunit.web
- xunit
- xunit.runner.visualstudio
- FluentAssertions
- Moq
- Microsoft.Extensions.DependencyInjection

**Test Categories**:
1. **Component Rendering Tests**: Test component output
   - Test HTML rendering
   - Test CSS class application
   - Test conditional rendering
   - Test component parameters

2. **Component Interaction Tests**: Test user interactions
   - Test button clicks
   - Test form submissions
   - Test input changes
   - Test event handling

3. **Component Lifecycle Tests**: Test component lifecycle
   - Test OnInitialized methods
   - Test parameter changes
   - Test disposal

4. **Service Integration Tests**: Test component-service interaction
   - Mock HTTP clients
   - Test data loading
   - Test error handling

**Test Structure**:
- Components/ (component tests organized by feature)
- Pages/ (page component tests)
- Shared/ (shared component tests)
- TestHelpers/ (bUnit test helpers and utilities)

**Additional Requirements**:
- Use bUnit TestContext for component testing
- Mock JavaScript interop calls
- Test responsive behavior
- Include accessibility testing
- Test component state management
```

### Prompt for Service Defaults Unit Tests

```
Create unit tests for the Testers.ServiceDefaults project:

**Project Name**: `ServiceDefaults.UnitTests`
**Target Framework**: .NET 9.0
**Focus Areas**:
- Extension method testing
- Configuration validation
- Health check implementations
- Telemetry setup
- Logging configuration

**Test Requirements**:
- Test service registration extensions
- Test configuration binding
- Test health check responses
- Mock IServiceCollection for extension testing
- Validate telemetry data collection
```

---

## 🔗 Integration Tests

### Prompt for API Integration Tests

```
Create comprehensive integration tests for the API with the following requirements:

**Project Name**: `Api.IntegrationTests`
**Target Framework**: .NET 9.0
**Test Framework**: xUnit + ASP.NET Core Test Host

**Required NuGet Packages**:
- Microsoft.NET.Test.Sdk
- Microsoft.AspNetCore.Mvc.Testing
- xunit
- xunit.runner.visualstudio
- FluentAssertions
- Testcontainers.PostgreSql
- Microsoft.Extensions.Configuration
- Microsoft.Extensions.Logging

**Test Categories**:
1. **HTTP API Tests**: Test complete request/response cycles
   - Test all API endpoints
   - Test authentication/authorization
   - Test request/response serialization
   - Test error responses and status codes

2. **Database Integration Tests**: Test data persistence
   - Test CRUD operations
   - Test database transactions
   - Test data consistency
   - Test migration scenarios

3. **Configuration Tests**: Test application configuration
   - Test environment-specific settings
   - Test connection string validation
   - Test feature flag behavior

4. **Middleware Tests**: Test custom middleware
   - Test request/response modification
   - Test authentication middleware
   - Test logging middleware
   - Test exception handling middleware

**Infrastructure Requirements**:
- Use TestContainers for PostgreSQL database
- Custom WebApplicationFactory for test setup
- Test-specific configuration files
- Database seeding for test data
- Transaction rollback for test isolation

**Test Organization**:
- Controllers/ (endpoint integration tests)
- Database/ (data access integration tests)
- Middleware/ (middleware integration tests)
- Configuration/ (config integration tests)
- Fixtures/ (test setup and shared fixtures)

**Additional Requirements**:
- Use test-specific database per test class
- Implement test data builders
- Test with realistic data volumes
- Include performance benchmarks
- Test concurrent access scenarios
```

### Prompt for Database Integration Tests

```
Create dedicated database integration tests:

**Project Name**: `Database.IntegrationTests`
**Focus**: Deep database testing with real PostgreSQL instance

**Test Areas**:
1. **Schema Tests**: Validate database schema
   - Test table creation
   - Test indexes and constraints
   - Test foreign key relationships
   - Test stored procedures/functions

2. **Migration Tests**: Test database migrations
   - Test forward migrations
   - Test rollback scenarios
   - Test data preservation during migrations
   - Test schema versioning

3. **Performance Tests**: Database performance
   - Test query performance
   - Test connection pooling
   - Test transaction performance
   - Test bulk operations

4. **Data Integrity Tests**: Test data consistency
   - Test ACID properties
   - Test concurrent access
   - Test deadlock handling
   - Test constraint validation

**Requirements**:
- Use Testcontainers for isolated PostgreSQL instances
- Include realistic test data sets
- Test with multiple database connections
- Measure query execution times
- Test backup and restore scenarios
```

### Prompt for Services Integration Tests

```
Create integration tests for service-to-service communication:

**Project Name**: `Services.IntegrationTests`
**Focus**: Test inter-service communication and workflows

**Test Scenarios**:
1. **Service Discovery Tests**: Test Aspire service discovery
   - Test service registration
   - Test service resolution
   - Test health check propagation
   - Test configuration distribution

2. **API-to-Database Tests**: Test complete data flow
   - Test API → Database operations
   - Test transaction management
   - Test error propagation
   - Test retry mechanisms

3. **Event-Driven Tests**: Test event handling (if applicable)
   - Test event publishing
   - Test event consumption
   - Test event ordering
   - Test event failure handling

4. **External Service Tests**: Test external dependencies
   - Mock external APIs
   - Test timeout scenarios
   - Test circuit breaker patterns
   - Test fallback mechanisms

**Infrastructure**:
- Use Aspire test host
- Docker compose for multi-service testing
- Test service orchestration
- Monitor telemetry data during tests
```

---

## 🎭 Functional Tests

### Prompt for Web UI Functional Tests

```
Create end-to-end functional tests for both React and Blazor frontends:

**Project Name**: `WebUI.FunctionalTests`
**Target Framework**: .NET 9.0
**Automation Framework**: Playwright for .NET

**Required NuGet Packages**:
- Microsoft.NET.Test.Sdk
- Microsoft.Playwright
- Microsoft.Playwright.NUnit (or xUnit)
- FluentAssertions
- Microsoft.Extensions.Configuration

**Test Categories**:
1. **User Journey Tests**: Complete user workflows
   - Test user registration/login flows
   - Test data entry and submission
   - Test navigation between pages
   - Test form validation scenarios

2. **Cross-Browser Tests**: Test browser compatibility
   - Test on Chromium, Firefox, Safari
   - Test responsive design
   - Test JavaScript functionality
   - Test CSS rendering

3. **API Integration Tests**: Frontend-to-API communication
   - Test data loading from API
   - Test form submission to API
   - Test error handling display
   - Test loading states

4. **Accessibility Tests**: Test accessibility compliance
   - Test keyboard navigation
   - Test screen reader compatibility
   - Test color contrast
   - Test ARIA attributes

**Page Object Model Structure**:
- Pages/ (page object models)
- Components/ (reusable component objects)
- Tests/ (test classes organized by feature)
- Fixtures/ (test setup and data)
- Utilities/ (helper methods and extensions)

**Test Organization by Frontend**:
- React/ (React-specific tests)
- Blazor/ (Blazor-specific tests)
- Shared/ (common functionality tests)

**Additional Requirements**:
- Implement Page Object Model pattern
- Use data-testid attributes for element selection
- Include visual regression testing
- Test both light and dark themes (if applicable)
- Test mobile and desktop viewports
- Record videos of failing tests
- Take screenshots on test failures
```

### Prompt for API Functional Tests

```
Create functional tests focused on API behavior:

**Project Name**: `API.FunctionalTests`
**Focus**: Black-box testing of API functionality

**Test Areas**:
1. **Business Logic Tests**: Test API business rules
   - Test complex business workflows
   - Test data validation rules
   - Test business constraint enforcement
   - Test calculation accuracy

2. **Security Tests**: Test API security features
   - Test authentication flows
   - Test authorization rules
   - Test input sanitization
   - Test rate limiting

3. **Data Flow Tests**: Test complete data scenarios
   - Test data creation workflows
   - Test data update scenarios
   - Test data deletion with dependencies
   - Test bulk operations

4. **Error Handling Tests**: Test error scenarios
   - Test validation error responses
   - Test system error handling
   - Test timeout scenarios
   - Test malformed request handling

**Requirements**:
- Use HttpClient for API calls
- Test against running application instance
- Include realistic test data scenarios
- Test with large data sets
- Validate response schemas
- Test API versioning scenarios
```

### Prompt for Workflow Functional Tests

```
Create tests for complete business workflows:

**Project Name**: `Workflow.FunctionalTests`
**Focus**: End-to-end business process testing

**Workflow Tests**:
1. **Complete User Journeys**: Full user scenarios
   - User registration → data entry → submission → verification
   - Error recovery workflows
   - Multi-step form completion
   - Data persistence across sessions

2. **System Integration Workflows**: Cross-system processes
   - Frontend → API → Database workflows
   - Authentication → authorization → data access
   - File upload → processing → storage
   - Notification and communication flows

3. **Administrative Workflows**: Admin user scenarios
   - User management workflows
   - System configuration changes
   - Monitoring and reporting workflows
   - Backup and recovery procedures

**Requirements**:
- Use real database instances
- Test with production-like data
- Include timing and performance validation
- Test concurrent user scenarios
- Validate audit trails and logging
```

---

## ⚡ Performance Tests

### Prompt for k6 Load Tests

```
Create JavaScript-based load tests using k6:

**Project Name**: `LoadTests.K6`
**Framework**: k6 (JavaScript)

**Test Scenarios**:
1. **Basic Load Tests**: Standard performance testing
   - Ramp-up tests (gradual load increase)
   - Sustained load tests (constant load)
   - Spike tests (sudden load increase)
   - Volume tests (large amounts of data)

2. **API Performance Tests**: API-specific load testing
   - Test all API endpoints under load
   - Test database operations under load
   - Test authentication under load
   - Test file operations under load

3. **Stress Tests**: System breaking point testing
   - Find maximum user capacity
   - Test memory usage under stress
   - Test CPU usage under stress
   - Test database connection limits

**File Structure**:
- scenarios/ (different test scenarios)
- data/ (test data files)
- config/ (k6 configuration files)
- reports/ (test result reports)
- utils/ (shared utility functions)

**Test Configuration**:
- Different VU (Virtual User) configurations
- Different duration settings
- Different ramp-up/ramp-down patterns
- Environment-specific configurations

**Metrics to Collect**:
- Response times (p95, p99)
- Request rate
- Error rate
- System resource usage
- Database performance metrics

**Requirements**:
- Create realistic user behavior scenarios
- Include data parameterization
- Test with production-like data volumes
- Generate comprehensive reports
- Include SLA validation
```

### Prompt for NBomber Performance Tests

```
Create C#-based performance tests using NBomber:

**Project Name**: `LoadTests.NBomber`
**Target Framework**: .NET 9.0
**Framework**: NBomber

**Required NuGet Packages**:
- NBomber
- NBomber.Http
- Microsoft.Extensions.Configuration
- FluentAssertions

**Test Categories**:
1. **HTTP Load Tests**: Web API performance testing
   - Test API endpoints with realistic payloads
   - Test concurrent user scenarios
   - Test different HTTP methods (GET, POST, PUT, DELETE)
   - Test with authentication tokens

2. **Database Load Tests**: Database performance testing
   - Test database read operations
   - Test database write operations
   - Test complex queries
   - Test transaction performance

3. **Mixed Workload Tests**: Combined scenario testing
   - Test realistic user workflows
   - Test mixed read/write operations
   - Test different user types simultaneously
   - Test peak usage scenarios

**Test Structure**:
- Scenarios/ (NBomber scenario definitions)
- Data/ (test data and configurations)
- Models/ (data models for testing)
- Reports/ (performance test reports)
- Utilities/ (helper classes and extensions)

**Performance Metrics**:
- Request/response metrics
- Throughput measurements
- Resource utilization
- Custom business metrics
- SLA compliance validation

**Additional Requirements**:
- Create reusable scenario templates
- Include real-time monitoring
- Generate detailed HTML reports
- Test with realistic data distributions
- Include performance regression detection
```

### Prompt for Stress Tests

```
Create specialized stress testing scenarios:

**Project Name**: `StressTests`
**Focus**: System breaking point and recovery testing

**Stress Test Types**:
1. **Resource Exhaustion Tests**: Test system limits
   - Memory stress tests
   - CPU stress tests
   - Disk I/O stress tests
   - Network bandwidth tests

2. **Concurrent User Stress**: Test user capacity limits
   - Maximum concurrent users
   - Database connection limits
   - Session management stress
   - Memory usage under user load

3. **Data Volume Stress**: Test with large data sets
   - Large file uploads
   - Bulk data processing
   - Large result set queries
   - Database table size limits

4. **Recovery Tests**: Test system recovery
   - Test recovery after crashes
   - Test graceful degradation
   - Test circuit breaker activation
   - Test auto-scaling behavior

**Requirements**:
- Monitor system resources during tests
- Test system recovery mechanisms
- Include chaos engineering scenarios
- Test backup and failover systems
- Validate error handling under stress
```

---

## 🔒 Security Tests

### Prompt for OWASP ZAP Security Tests

```
Create automated security testing using OWASP ZAP:

**Project Name**: `PenetrationTests.ZAP`
**Tool**: OWASP ZAP with automation

**Security Test Categories**:
1. **Web Application Security**: Standard web security testing
   - Test for OWASP Top 10 vulnerabilities
   - Test input validation and sanitization
   - Test authentication and session management
   - Test authorization and access controls

2. **API Security Testing**: REST API specific security
   - Test API authentication mechanisms
   - Test API authorization rules
   - Test input validation on API endpoints
   - Test for API-specific vulnerabilities

3. **Configuration Security**: Test security configurations
   - Test HTTPS configuration
   - Test security headers
   - Test cookie security settings
   - Test CORS configuration

**ZAP Test Configuration**:
- Automated spider crawling
- Active security scanning
- Passive security analysis
- Custom attack scenarios

**Test Automation**:
- Integrate ZAP with CI/CD pipeline
- Generate security reports
- Set security baseline thresholds
- Alert on new vulnerabilities

**Requirements**:
- Create ZAP automation scripts
- Include custom security test cases
- Generate detailed security reports
- Track security metrics over time
- Include remediation guidance
```

### Prompt for API Security Tests

```
Create dedicated API security tests:

**Project Name**: `ApiSecurity.Tests`
**Target Framework**: .NET 9.0
**Focus**: API-specific security testing

**Security Test Areas**:
1. **Authentication Tests**: Test auth mechanisms
   - Test JWT token validation
   - Test token expiration handling
   - Test refresh token security
   - Test multi-factor authentication

2. **Authorization Tests**: Test access controls
   - Test role-based access control
   - Test resource-level permissions
   - Test privilege escalation attempts
   - Test cross-user data access

3. **Input Validation Tests**: Test injection attacks
   - Test SQL injection attempts
   - Test NoSQL injection attempts
   - Test XSS attempts in API responses
   - Test command injection attempts

4. **Rate Limiting Tests**: Test DoS protection
   - Test API rate limiting
   - Test throttling mechanisms
   - Test IP-based blocking
   - Test user-based limits

**Test Implementation**:
- Use security testing frameworks
- Include negative test cases
- Test with malicious payloads
- Validate security headers
- Test encryption in transit
```

### Prompt for Authentication Tests

```
Create comprehensive authentication and authorization tests:

**Project Name**: `AuthenticationTests`
**Focus**: Identity and access management testing

**Test Categories**:
1. **Authentication Flow Tests**: Test login mechanisms
   - Test valid login scenarios
   - Test invalid login attempts
   - Test account lockout mechanisms
   - Test password reset flows

2. **Session Management Tests**: Test session security
   - Test session creation and destruction
   - Test session timeout handling
   - Test concurrent session limits
   - Test session hijacking protection

3. **Token Management Tests**: Test token-based auth
   - Test JWT token generation
   - Test token validation
   - Test token refresh mechanisms
   - Test token revocation

4. **Authorization Tests**: Test access control
   - Test role assignments
   - Test permission inheritance
   - Test resource access controls
   - Test administrative functions

**Requirements**:
- Test with real identity providers
- Include multi-tenant scenarios
- Test with different user roles
- Validate audit logging
- Test compliance requirements
```

---

## 📋 Contract Tests

### Prompt for API Contract Tests

```
Create API contract tests to ensure API compatibility:

**Project Name**: `ApiContract.Tests`
**Framework**: Pact.NET or custom contract testing

**Contract Testing Areas**:
1. **API Schema Validation**: Test API contract compliance
   - Test OpenAPI specification compliance
   - Test request/response schema validation
   - Test API versioning compatibility
   - Test backward compatibility

2. **Consumer-Driven Tests**: Test from consumer perspective
   - Test frontend-API contracts
   - Test service-to-service contracts
   - Test third-party integration contracts
   - Test mobile app API contracts

3. **Provider Verification**: Test API provider compliance
   - Verify API matches published contracts
   - Test contract evolution scenarios
   - Test breaking change detection
   - Test SLA compliance

**Contract Test Structure**:
- Contracts/ (contract definitions)
- Consumer/ (consumer contract tests)
- Provider/ (provider verification tests)
- Shared/ (shared contract utilities)

**Requirements**:
- Generate contract specifications
- Validate against multiple API versions
- Include contract evolution testing
- Generate compatibility reports
- Integrate with CI/CD pipeline
```

### Prompt for Database Contract Tests

```
Create database contract tests:

**Project Name**: `DatabaseContract.Tests`
**Focus**: Database schema and data contract testing

**Contract Areas**:
1. **Schema Contract Tests**: Test database schema
   - Test table structure compliance
   - Test column definitions
   - Test constraint validation
   - Test index definitions

2. **Data Contract Tests**: Test data expectations
   - Test data format validation
   - Test data relationship integrity
   - Test data migration compatibility
   - Test stored procedure contracts

3. **Migration Contract Tests**: Test schema evolution
   - Test forward migration compatibility
   - Test rollback scenarios
   - Test data preservation
   - Test performance impact

**Requirements**:
- Compare schema against baseline
- Validate data types and constraints
- Test with realistic data volumes
- Include performance validation
- Generate schema documentation
```

---

## 🚀 Getting Started Commands

### Create All Test Projects

```powershell
# Navigate to the solution root
cd g:\Git\AB.TestersPlayground

# Create test directories
mkdir tests\unit, tests\integration, tests\functional, tests\performance, tests\security, tests\contract

# Create Unit Test Projects
dotnet new xunit -n Api.UnitTests -o tests\unit\Api.UnitTests
dotnet new xunit -n Blazor.UnitTests -o tests\unit\Blazor.UnitTests  
dotnet new xunit -n ServiceDefaults.UnitTests -o tests\unit\ServiceDefaults.UnitTests

# Create Integration Test Projects
dotnet new xunit -n Api.IntegrationTests -o tests\integration\Api.IntegrationTests
dotnet new xunit -n Database.IntegrationTests -o tests\integration\Database.IntegrationTests
dotnet new xunit -n Services.IntegrationTests -o tests\integration\Services.IntegrationTests

# Create Functional Test Projects
dotnet new xunit -n WebUI.FunctionalTests -o tests\functional\WebUI.FunctionalTests
dotnet new xunit -n API.FunctionalTests -o tests\functional\API.FunctionalTests
dotnet new xunit -n Workflow.FunctionalTests -o tests\functional\Workflow.FunctionalTests

# Create Performance Test Projects  
dotnet new console -n LoadTests.NBomber -o tests\performance\LoadTests.NBomber
mkdir tests\performance\LoadTests.K6
dotnet new xunit -n StressTests -o tests\performance\StressTests

# Create Security Test Projects
mkdir tests\security\PenetrationTests.ZAP
dotnet new xunit -n ApiSecurity.Tests -o tests\security\ApiSecurity.Tests
dotnet new xunit -n AuthenticationTests -o tests\security\AuthenticationTests

# Create Contract Test Projects
dotnet new xunit -n ApiContract.Tests -o tests\contract\ApiContract.Tests
dotnet new xunit -n DatabaseContract.Tests -o tests\contract\DatabaseContract.Tests

# Add projects to solution
dotnet sln add tests\unit\Api.UnitTests\Api.UnitTests.csproj
dotnet sln add tests\unit\Blazor.UnitTests\Blazor.UnitTests.csproj
dotnet sln add tests\unit\ServiceDefaults.UnitTests\ServiceDefaults.UnitTests.csproj
dotnet sln add tests\integration\Api.IntegrationTests\Api.IntegrationTests.csproj
dotnet sln add tests\integration\Database.IntegrationTests\Database.IntegrationTests.csproj
dotnet sln add tests\integration\Services.IntegrationTests\Services.IntegrationTests.csproj
dotnet sln add tests\functional\WebUI.FunctionalTests\WebUI.FunctionalTests.csproj
dotnet sln add tests\functional\API.FunctionalTests\API.FunctionalTests.csproj
dotnet sln add tests\functional\Workflow.FunctionalTests\Workflow.FunctionalTests.csproj
dotnet sln add tests\performance\LoadTests.NBomber\LoadTests.NBomber.csproj
dotnet sln add tests\performance\StressTests\StressTests.csproj
dotnet sln add tests\security\ApiSecurity.Tests\ApiSecurity.Tests.csproj
dotnet sln add tests\security\AuthenticationTests\AuthenticationTests.csproj
dotnet sln add tests\contract\ApiContract.Tests\ApiContract.Tests.csproj
dotnet sln add tests\contract\DatabaseContract.Tests\DatabaseContract.Tests.csproj
```

### Install Required NuGet Packages

```powershell
# Add common test packages to all xUnit projects
$testProjects = @(
    "tests\unit\Api.UnitTests\Api.UnitTests.csproj",
    "tests\unit\Blazor.UnitTests\Blazor.UnitTests.csproj",
    "tests\unit\ServiceDefaults.UnitTests\ServiceDefaults.UnitTests.csproj",
    "tests\integration\Api.IntegrationTests\Api.IntegrationTests.csproj",
    "tests\integration\Database.IntegrationTests\Database.IntegrationTests.csproj",
    "tests\integration\Services.IntegrationTests\Services.IntegrationTests.csproj",
    "tests\functional\WebUI.FunctionalTests\WebUI.FunctionalTests.csproj",
    "tests\functional\API.FunctionalTests\API.FunctionalTests.csproj",
    "tests\functional\Workflow.FunctionalTests\Workflow.FunctionalTests.csproj",
    "tests\performance\StressTests\StressTests.csproj",
    "tests\security\ApiSecurity.Tests\ApiSecurity.Tests.csproj",
    "tests\security\AuthenticationTests\AuthenticationTests.csproj",
    "tests\contract\ApiContract.Tests\ApiContract.Tests.csproj",
    "tests\contract\DatabaseContract.Tests\DatabaseContract.Tests.csproj"
)

foreach ($project in $testProjects) {
    dotnet add $project package FluentAssertions
    dotnet add $project package Moq
}

# Add specific packages for Blazor tests
dotnet add tests\unit\Blazor.UnitTests\Blazor.UnitTests.csproj package bunit
dotnet add tests\unit\Blazor.UnitTests\Blazor.UnitTests.csproj package bunit.web

# Add packages for integration tests
dotnet add tests\integration\Api.IntegrationTests\Api.IntegrationTests.csproj package Microsoft.AspNetCore.Mvc.Testing
dotnet add tests\integration\Database.IntegrationTests\Database.IntegrationTests.csproj package Testcontainers.PostgreSql

# Add packages for functional tests
dotnet add tests\functional\WebUI.FunctionalTests\WebUI.FunctionalTests.csproj package Microsoft.Playwright
dotnet add tests\functional\WebUI.FunctionalTests\WebUI.FunctionalTests.csproj package Microsoft.Playwright.NUnit

# Add packages for performance tests
dotnet add tests\performance\LoadTests.NBomber\LoadTests.NBomber.csproj package NBomber
dotnet add tests\performance\LoadTests.NBomber\LoadTests.NBomber.csproj package NBomber.Http
```

---

## 🎯 Next Steps

1. **Choose Your Starting Point**: Select the test type most relevant to your current needs
2. **Use the Prompts**: Copy the detailed prompts above to create comprehensive test projects
3. **Customize for Your Needs**: Adapt the test requirements to match your specific application requirements
4. **Run the Commands**: Use the PowerShell commands to quickly scaffold all test projects
5. **Implement Gradually**: Start with unit tests and gradually add more complex test types

## 📚 Additional Resources

- **xUnit Documentation**: https://xunit.net/
- **bUnit Documentation**: https://bunit.dev/
- **Playwright Documentation**: https://playwright.dev/dotnet/
- **NBomber Documentation**: https://nbomber.com/
- **k6 Documentation**: https://k6.io/docs/
- **OWASP ZAP Documentation**: https://www.zaproxy.org/docs/
- **Testcontainers Documentation**: https://testcontainers.com/
- **ASP.NET Core Testing**: https://docs.microsoft.com/en-us/aspnet/core/test/

---

**🎉 Ready to Build Comprehensive Test Coverage!**

Use these prompts to create a robust testing ecosystem that covers all aspects of your application, from individual units to complete user workflows. Each test type serves a specific purpose in ensuring the quality, security, and performance of your AB.TestersPlayground application.
