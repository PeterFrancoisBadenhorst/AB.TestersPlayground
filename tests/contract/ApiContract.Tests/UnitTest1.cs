using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace ApiContract.Tests;

/// <summary>
/// API Contract Tests - Verify API contract compliance
/// Tests that API endpoints maintain their expected contracts
/// </summary>
public class WeatherApiContractTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public WeatherApiContractTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GET_WeatherForecast_ReturnsExpectedSchema()
    {
        // Act
        var response = await _client.GetAsync("/weatherforecast");
        var jsonContent = await response.Content.ReadAsStringAsync();
        var weatherData = JsonSerializer.Deserialize<WeatherForecast[]>(jsonContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Assert - Verify contract compliance
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/json");
        
        weatherData.Should().NotBeNull();
        weatherData.Should().HaveCount(5); // Contract: Always returns 5 items
        
        foreach (var forecast in weatherData!)
        {
            // Contract: Each forecast must have these properties
            forecast.Date.Should().BeAfter(DateTime.Today.AddDays(-1));
            forecast.TemperatureC.Should().BeInRange(-100, 100);
            forecast.TemperatureF.Should().BeInRange(-200, 200);
            forecast.Summary.Should().NotBeNullOrEmpty();
            
            // Contract: TemperatureF should be calculated from TemperatureC
            var expectedF = 32 + (int)(forecast.TemperatureC / 0.5556);
            forecast.TemperatureF.Should().BeCloseTo(expectedF, 2);
        }
    }

    [Fact]
    public async Task GET_Health_ReturnsExpectedFormat()
    {
        // Act
        var response = await _client.GetAsync("/health");
        var content = await response.Content.ReadAsStringAsync();

        // Assert - Verify health check contract
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.Should().NotBeNullOrEmpty();
        
        // Contract: Health endpoint should return status information
        content.Should().MatchRegex("(Healthy|Unhealthy|Degraded)");
    }

    [Fact]
    public async Task GET_Info_ReturnsApplicationMetadata()
    {
        // Act
        var response = await _client.GetAsync("/info");

        // Assert - Verify info endpoint contract
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be("text/plain");
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GET_Test_ReturnsExpectedMessage()
    {
        // Act
        var response = await _client.GetAsync("/test");
        var content = await response.Content.ReadAsStringAsync();

        // Assert - Verify test endpoint contract
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be("text/plain");
        content.Should().Contain("Hello from Test endpoint");
    }

    [Theory]
    [InlineData("GET", "/weatherforecast")]
    [InlineData("GET", "/health")]
    [InlineData("GET", "/info")]
    [InlineData("GET", "/test")]
    public async Task API_Endpoints_ShouldSupportCORS(string method, string endpoint)
    {
        // Arrange
        var request = new HttpRequestMessage(new HttpMethod(method), endpoint);
        request.Headers.Add("Origin", "https://localhost:3000");

        // Act
        var response = await _client.SendAsync(request);

        // Assert - Verify CORS contract
        response.Headers.Should().ContainKey("Access-Control-Allow-Origin");
    }

    [Fact]
    public async Task API_ShouldReturnConsistentErrorFormat()
    {
        // Act
        var response = await _client.GetAsync("/nonexistent-endpoint");

        // Assert - Verify error response contract
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
        // Contract: Error responses should be consistent
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task WeatherForecast_Properties_ShouldMaintainDataTypes()
    {
        // Act
        var response = await _client.GetAsync("/weatherforecast");
        var jsonDocument = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());

        // Assert - Verify JSON schema contract
        jsonDocument.RootElement.Should().BeOfType<JsonElement>();
        jsonDocument.RootElement.ValueKind.Should().Be(JsonValueKind.Array);
        
        foreach (var item in jsonDocument.RootElement.EnumerateArray())
        {
            item.GetProperty("date").ValueKind.Should().Be(JsonValueKind.String);
            item.GetProperty("temperatureC").ValueKind.Should().Be(JsonValueKind.Number);
            item.GetProperty("temperatureF").ValueKind.Should().Be(JsonValueKind.Number);
            item.GetProperty("summary").ValueKind.Should().Be(JsonValueKind.String);
        }
    }
}

/// <summary>
/// Contract model for weather forecast
/// Must match the API response structure
/// </summary>
public class WeatherForecast
{
    public DateTime Date { get; set; }
    public int TemperatureC { get; set; }
    public int TemperatureF { get; set; }
    public string Summary { get; set; } = string.Empty;
}
