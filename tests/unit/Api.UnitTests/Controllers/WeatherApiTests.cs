using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.Json;
using Api.UnitTests.Helpers;

namespace Api.UnitTests.Controllers;

/// <summary>
/// Integration tests for the Weather API endpoints
/// Tests the complete request/response cycle for weather-related endpoints
/// </summary>
public class WeatherApiTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public WeatherApiTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetWeatherForecast_ReturnsSuccessResponse()
    {
        // Act
        var response = await _client.GetAsync("/weatherforecast");

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetWeatherForecast_ReturnsValidJsonContent()
    {
        // Act
        var response = await _client.GetAsync("/weatherforecast");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().NotBeNullOrEmpty();
        
        // Verify it's valid JSON array
        var weatherData = JsonSerializer.Deserialize<JsonElement>(content);
        weatherData.ValueKind.Should().Be(JsonValueKind.Array);
        weatherData.GetArrayLength().Should().Be(5);
    }

    [Fact]
    public async Task GetWeatherForecast_ReturnsCorrectContentType()
    {
        // Act
        var response = await _client.GetAsync("/weatherforecast");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/json");
    }

    [Theory]
    [InlineData("/weatherforecast")]
    [InlineData("/test")]
    [InlineData("/info")]
    public async Task ApiEndpoints_ReturnSuccessStatusCodes(string endpoint)
    {
        // Act
        var response = await _client.GetAsync(endpoint);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetTest_ReturnsHealthMessage()
    {
        // Act
        var response = await _client.GetAsync("/test");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().Contain("API is working!");
        content.Should().Contain("Current time:");
    }

    [Fact]
    public async Task GetInfo_ReturnsApiInformation()
    {
        // Act
        var response = await _client.GetAsync("/info");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var infoData = JsonSerializer.Deserialize<JsonElement>(content);
        infoData.GetProperty("Service").GetString().Should().Be("Testers Playground API");
        infoData.GetProperty("Version").GetString().Should().Be("1.0.0");
        infoData.TryGetProperty("Environment", out _).Should().BeTrue();
        infoData.TryGetProperty("Timestamp", out _).Should().BeTrue();
        infoData.TryGetProperty("MachineName", out _).Should().BeTrue();
    }

    [Fact]
    public async Task HealthEndpoint_ReturnsHealthy()
    {
        // Act
        var response = await _client.GetAsync("/health");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task WeatherForecast_ResponseTime_IsAcceptable()
    {
        // Arrange
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        // Act
        var response = await _client.GetAsync("/weatherforecast");
        stopwatch.Stop();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(1000); // Should respond within 1 second
    }
}
