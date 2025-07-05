using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text.Json;
using Xunit;

namespace Api.IntegrationTests.Controllers;

/// <summary>
/// Integration tests for API endpoints
/// Demonstrates testing complete request/response cycles
/// </summary>
public class ApiEndpointsIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public ApiEndpointsIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GET_WeatherForecast_ReturnsSuccessAndCorrectContentType()
    {
        // Act
        var response = await _client.GetAsync("/weatherforecast");

        // Assert
        response.EnsureSuccessStatusCode();
        response.Content.Headers.ContentType?.ToString().Should().Contain("application/json");
    }

    [Fact]
    public async Task GET_WeatherForecast_ReturnsValidWeatherData()
    {
        // Act
        var response = await _client.GetAsync("/weatherforecast");
        var content = await response.Content.ReadAsStringAsync();
        var weatherData = JsonSerializer.Deserialize<WeatherForecast[]>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        weatherData.Should().NotBeNull();
        weatherData.Should().HaveCount(5);
        weatherData.Should().OnlyContain(w => w.Date >= DateTime.Today);
        weatherData.Should().OnlyContain(w => !string.IsNullOrEmpty(w.Summary));
    }

    [Fact]
    public async Task GET_Health_ReturnsHealthy()
    {
        // Act
        var response = await _client.GetAsync("/health");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Healthy");
    }

    [Fact]
    public async Task GET_Info_ReturnsApplicationInfo()
    {
        // Act
        var response = await _client.GetAsync("/info");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GET_Test_ReturnsExpectedMessage()
    {
        // Act
        var response = await _client.GetAsync("/test");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Hello from Test endpoint");
    }

    [Theory]
    [InlineData("/nonexistent")]
    [InlineData("/api/invalid")]
    [InlineData("/random-endpoint")]
    public async Task GET_NonExistentEndpoints_ReturnsNotFound(string endpoint)
    {
        // Act
        var response = await _client.GetAsync(endpoint);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Multiple_Concurrent_Requests_ShouldSucceed()
    {
        // Arrange
        var tasks = new List<Task<HttpResponseMessage>>();
        
        // Act
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(_client.GetAsync("/weatherforecast"));
        }
        
        var responses = await Task.WhenAll(tasks);

        // Assert
        responses.Should().HaveCount(10);
        responses.Should().OnlyContain(r => r.IsSuccessStatusCode);
    }
}

public class WeatherForecast
{
    public DateTime Date { get; set; }
    public int TemperatureC { get; set; }
    public int TemperatureF { get; set; }
    public string Summary { get; set; } = string.Empty;
}
