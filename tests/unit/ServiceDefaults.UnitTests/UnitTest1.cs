using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Xunit;

namespace ServiceDefaults.UnitTests;

/// <summary>
/// Unit tests for ServiceDefaults extension methods
/// Demonstrates testing service registration and configuration extensions
/// </summary>
public class ServiceDefaultsExtensionsTests
{
    [Fact]
    public void AddServiceDefaults_ShouldRegisterRequiredServices()
    {
        // Arrange
        var services = new ServiceCollection();
        var builder = Host.CreateApplicationBuilder();

        // Act
        builder.AddServiceDefaults();

        // Assert
        builder.Services.Should().NotBeEmpty();
        // Verify that health checks are registered
        builder.Services.Should().Contain(x => x.ServiceType.Name.Contains("HealthCheck"));
    }

    [Fact]
    public void AddServiceDefaults_ShouldConfigureTelemetry()
    {
        // Arrange
        var builder = Host.CreateApplicationBuilder();

        // Act
        builder.AddServiceDefaults();

        // Assert
        // Verify that telemetry services are registered
        builder.Services.Should().Contain(x => x.ServiceType.Name.Contains("Telemetry") || 
                                              x.ServiceType.Name.Contains("OpenTelemetry"));
    }

    [Fact]
    public void AddServiceDefaults_ShouldConfigureHealthChecks()
    {
        // Arrange
        var builder = Host.CreateApplicationBuilder();

        // Act
        builder.AddServiceDefaults();

        // Assert
        // Build the service provider and verify health checks are configured
        var serviceProvider = builder.Services.BuildServiceProvider();
        var healthCheckService = serviceProvider.GetService<Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckService>();
        healthCheckService.Should().NotBeNull();
    }

    [Fact]
    public void ConfigureOpenTelemetry_ShouldSetupProperConfiguration()
    {
        // Arrange
        var builder = Host.CreateApplicationBuilder();

        // Act
        builder.ConfigureOpenTelemetry();

        // Assert
        // Verify that OpenTelemetry services are registered
        builder.Services.Should().Contain(x => x.ServiceType.Name.Contains("OpenTelemetry"));
    }

    [Fact]
    public void ServiceDefaults_ExtensionMethods_ShouldBeAvailable()
    {
        // Arrange
        var builder = Host.CreateApplicationBuilder();

        // Act & Assert
        var act1 = () => builder.AddServiceDefaults();
        var act2 = () => builder.ConfigureOpenTelemetry();
        
        act1.Should().NotThrow();
        act2.Should().NotThrow();
    }
}
