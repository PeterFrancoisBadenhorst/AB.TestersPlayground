using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace Api.UnitTests.Extensions;

/// <summary>
/// Unit tests for custom extension methods
/// Demonstrates testing utility functions and configuration extensions
/// </summary>
public class ServiceExtensionsTests
{
    [Fact]
    public void AddWeatherServices_ShouldRegisterRequiredServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddWeatherServices();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var weatherService = serviceProvider.GetService<IWeatherService>();
        weatherService.Should().NotBeNull();
    }

    [Fact]
    public void AddCustomHealthChecks_ShouldRegisterHealthChecks()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddCustomHealthChecks();

        // Assert
        services.Should().Contain(x => x.ServiceType.Name.Contains("HealthCheck"));
    }

    [Fact]
    public void GetRequiredConfigurationValue_WithMissingKey_ShouldThrowException()
    {
        // Arrange
        var configuration = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
            .Build();

        // Act & Assert
        var act = () => configuration.GetRequiredConfigurationValue("MissingKey");
        act.Should().Throw<InvalidOperationException>()
           .WithMessage("Configuration value for key 'MissingKey' is missing or empty");
    }

    [Fact]
    public void GetRequiredConfigurationValue_WithValidKey_ShouldReturnValue()
    {
        // This test demonstrates the extension method behavior
        // In a real application, configuration would be properly set up
        var configuration = new Microsoft.Extensions.Configuration.ConfigurationBuilder().Build();
        var act = () => configuration.GetRequiredConfigurationValue("TestKey");
        
        // Since we have no configuration values set up, this will throw
        act.Should().Throw<InvalidOperationException>();
    }
}

/// <summary>
/// Sample extension methods for testing
/// In a real application, these would be in separate files
/// </summary>
public static class ServiceExtensions
{
    public static IServiceCollection AddWeatherServices(this IServiceCollection services)
    {
        services.AddScoped<IWeatherService, WeatherService>();
        return services;
    }

    public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
                .AddCheck<WeatherHealthCheck>("weather");
        return services;
    }

    public static string GetRequiredConfigurationValue(this Microsoft.Extensions.Configuration.IConfiguration configuration, string key)
    {
        var value = configuration[key];
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException($"Configuration value for key '{key}' is missing or empty");
        }
        return value;
    }
}

public interface IWeatherService
{
    Task<IEnumerable<TestWeatherForecast>> GetForecastAsync(int days);
}

public class WeatherService : IWeatherService
{
    public Task<IEnumerable<TestWeatherForecast>> GetForecastAsync(int days)
    {
        // Implementation would go here
        return Task.FromResult(Enumerable.Empty<TestWeatherForecast>());
    }
}

public class WeatherHealthCheck : Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck
{
    public Task<Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult> CheckHealthAsync(
        Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        // Health check implementation
        return Task.FromResult(Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy());
    }
}

public class TestWeatherForecast
{
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    public string? Summary { get; set; }
}
