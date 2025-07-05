using FluentAssertions;
using Xunit;

namespace Api.UnitTests.Services;

/// <summary>
/// Unit tests for weather service business logic
/// Demonstrates testing service layer with mock dependencies
/// </summary>
public class WeatherServiceTests
{
    [Fact]
    public void GetWeatherForecast_WhenCalled_ReturnsValidForecast()
    {
        // Arrange
        var service = new WeatherService();
        
        // Act
        var result = service.GetWeatherForecast(5);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(5);
        result.All(x => x.Date >= DateOnly.FromDateTime(DateTime.Today)).Should().BeTrue();
        result.All(x => x.TemperatureC >= -20 && x.TemperatureC <= 55).Should().BeTrue();
        result.All(x => !string.IsNullOrEmpty(x.Summary)).Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10)]
    public void GetWeatherForecast_WithValidDays_ReturnsCorrectCount(int days)
    {
        // Arrange
        var service = new WeatherService();
        
        // Act
        var result = service.GetWeatherForecast(days);
        
        // Assert
        result.Should().HaveCount(days);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-10)]
    public void GetWeatherForecast_WithNegativeDays_ThrowsArgumentException(int days)
    {
        // Arrange
        var service = new WeatherService();
        
        // Act & Assert
        var act = () => service.GetWeatherForecast(days);
        act.Should().Throw<ArgumentException>()
           .WithMessage("Days cannot be negative*");
    }
}

/// <summary>
/// Simple weather service for demonstration
/// In a real application, this would be injected as an interface
/// </summary>
public class WeatherService
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public IEnumerable<TestWeatherForecast> GetWeatherForecast(int days)
    {
        if (days < 0)
            throw new ArgumentException("Days cannot be negative", nameof(days));

        return Enumerable.Range(1, days).Select(index => new TestWeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Today.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}

public class TestWeatherForecast
{
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    public string? Summary { get; set; }
}
