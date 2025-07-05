using FluentAssertions;
using Xunit;
using System.ComponentModel.DataAnnotations;

namespace Api.UnitTests.Models;

/// <summary>
/// Unit tests for API data models and DTOs
/// Demonstrates testing model validation and behavior
/// </summary>
public class ApiModelsTests
{
    [Fact]
    public void WeatherForecast_WithValidData_ShouldPassValidation()
    {
        // Arrange
        var model = new WeatherForecastDto
        {
            Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            TemperatureC = 25,
            Summary = "Warm"
        };

        // Act
        var validationResults = ValidateModel(model);

        // Assert
        validationResults.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void WeatherForecast_WithInvalidSummary_ShouldFailValidation(string summary)
    {
        // Arrange
        var model = new WeatherForecastDto
        {
            Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            TemperatureC = 25,
            Summary = summary
        };

        // Act
        var validationResults = ValidateModel(model);

        // Assert
        validationResults.Should().NotBeEmpty();
        validationResults.Should().ContainSingle(x => x.MemberNames.Contains(nameof(WeatherForecastDto.Summary)));
    }

    [Theory]
    [InlineData(-100)]
    [InlineData(100)]
    public void WeatherForecast_WithTemperatureOutOfRange_ShouldFailValidation(int temperature)
    {
        // Arrange
        var model = new WeatherForecastDto
        {
            Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            TemperatureC = temperature,
            Summary = "Test"
        };

        // Act
        var validationResults = ValidateModel(model);

        // Assert
        validationResults.Should().NotBeEmpty();
        validationResults.Should().ContainSingle(x => x.MemberNames.Contains(nameof(WeatherForecastDto.TemperatureC)));
    }

    [Fact]
    public void WeatherForecast_TemperatureF_ShouldCalculateCorrectly()
    {
        // Arrange
        var model = new WeatherForecastDto
        {
            Date = DateOnly.FromDateTime(DateTime.Today),
            TemperatureC = 0,
            Summary = "Freezing"
        };

        // Act
        var temperatureF = model.TemperatureF;

        // Assert
        temperatureF.Should().Be(32);
    }

    [Theory]
    [InlineData(0, 32)]
    [InlineData(100, 212)]
    [InlineData(-40, -40)]
    public void WeatherForecast_TemperatureF_ShouldConvertCelsiusToFahrenheit(int celsius, int expectedFahrenheit)
    {
        // Arrange
        var model = new WeatherForecastDto
        {
            Date = DateOnly.FromDateTime(DateTime.Today),
            TemperatureC = celsius,
            Summary = "Test"
        };

        // Act
        var temperatureF = model.TemperatureF;

        // Assert
        temperatureF.Should().Be(expectedFahrenheit);
    }

    private static IList<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, context, validationResults, true);
        return validationResults;
    }
}

/// <summary>
/// Sample DTO model with validation attributes
/// </summary>
public class WeatherForecastDto
{
    public DateOnly Date { get; set; }

    [Range(-50, 60, ErrorMessage = "Temperature must be between -50 and 60 degrees Celsius")]
    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    [Required(ErrorMessage = "Summary is required")]
    [StringLength(100, ErrorMessage = "Summary cannot exceed 100 characters")]
    public string Summary { get; set; } = string.Empty;
}
