using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Blazor.UnitTests.Shared;

/// <summary>
/// Unit tests for shared components
/// Demonstrates testing component parameters, events, and service integration
/// </summary>
public class WeatherComponentTests : TestContext
{
    [Fact]
    public void WeatherComponent_WhenLoading_ShouldShowLoadingMessage()
    {
        // Arrange
        var mockWeatherService = new Mock<IWeatherService>();
        mockWeatherService.Setup(x => x.GetForecastAsync(It.IsAny<int>()))
                         .Returns(Task.Delay(1000).ContinueWith(_ => Enumerable.Empty<WeatherForecast>()));
        
        Services.AddSingleton(mockWeatherService.Object);

        // Act
        var component = RenderComponent<WeatherComponent>();

        // Assert
        component.Find("[data-testid='loading']").Should().NotBeNull();
        component.Markup.Should().Contain("Loading weather data...");
    }

    [Fact]
    public void WeatherComponent_WithData_ShouldDisplayWeatherForecast()
    {
        // Arrange
        var mockWeatherService = new Mock<IWeatherService>();
        var testData = new[]
        {
            new WeatherForecast { Date = DateOnly.FromDateTime(DateTime.Today), TemperatureC = 25, Summary = "Warm" },
            new WeatherForecast { Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)), TemperatureC = 15, Summary = "Cool" }
        };
        
        mockWeatherService.Setup(x => x.GetForecastAsync(It.IsAny<int>()))
                         .ReturnsAsync(testData);
        
        Services.AddSingleton(mockWeatherService.Object);

        // Act
        var component = RenderComponent<WeatherComponent>();

        // Assert
        component.WaitForState(() => component.FindAll("tr").Count > 1);
        component.FindAll("tr").Count.Should().Be(3); // Header + 2 data rows
        component.Markup.Should().Contain("Warm");
        component.Markup.Should().Contain("Cool");
    }

    [Fact]
    public void WeatherComponent_WithErrorFromService_ShouldShowErrorMessage()
    {
        // Arrange
        var mockWeatherService = new Mock<IWeatherService>();
        mockWeatherService.Setup(x => x.GetForecastAsync(It.IsAny<int>()))
                         .ThrowsAsync(new InvalidOperationException("Service unavailable"));
        
        Services.AddSingleton(mockWeatherService.Object);

        // Act
        var component = RenderComponent<WeatherComponent>();

        // Assert
        component.WaitForState(() => component.Find("[data-testid='error']") != null);
        component.Find("[data-testid='error']").TextContent.Should().Contain("Error loading weather data");
    }

    [Fact]
    public void WeatherComponent_RefreshButton_ShouldReloadData()
    {
        // Arrange
        var mockWeatherService = new Mock<IWeatherService>();
        mockWeatherService.Setup(x => x.GetForecastAsync(It.IsAny<int>()))
                         .ReturnsAsync(new[] { new WeatherForecast { Date = DateOnly.FromDateTime(DateTime.Today), TemperatureC = 20, Summary = "Test" } });
        
        Services.AddSingleton(mockWeatherService.Object);
        var component = RenderComponent<WeatherComponent>();

        // Act
        component.WaitForState(() => component.Find("[data-testid='refresh-button']") != null);
        var refreshButton = component.Find("[data-testid='refresh-button']");
        refreshButton.Click();

        // Assert
        mockWeatherService.Verify(x => x.GetForecastAsync(It.IsAny<int>()), Times.AtLeast(2));
    }
}

/// <summary>
/// Sample weather component for testing
/// In a real application, this would be a separate .razor file
/// </summary>
public partial class WeatherComponent : Microsoft.AspNetCore.Components.ComponentBase
{
    [Microsoft.AspNetCore.Components.Inject]
    public IWeatherService WeatherService { get; set; } = default!;

    private IEnumerable<WeatherForecast>? _forecasts;
    private bool _loading = true;
    private string? _errorMessage;

    protected override async Task OnInitializedAsync()
    {
        await LoadWeatherData();
    }

    private async Task LoadWeatherData()
    {
        try
        {
            _loading = true;
            _errorMessage = null;
            StateHasChanged();

            _forecasts = await WeatherService.GetForecastAsync(5);
        }
        catch (Exception ex)
        {
            _errorMessage = $"Error loading weather data: {ex.Message}";
        }
        finally
        {
            _loading = false;
            StateHasChanged();
        }
    }

    private async Task RefreshData()
    {
        await LoadWeatherData();
    }

    protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "class", "weather-component");

        // Title
        builder.OpenElement(2, "h3");
        builder.AddContent(3, "Weather Forecast");
        builder.CloseElement();

        if (_loading)
        {
            builder.OpenElement(4, "p");
            builder.AddAttribute(5, "data-testid", "loading");
            builder.AddContent(6, "Loading weather data...");
            builder.CloseElement();
        }
        else if (!string.IsNullOrEmpty(_errorMessage))
        {
            builder.OpenElement(7, "div");
            builder.AddAttribute(8, "data-testid", "error");
            builder.AddAttribute(9, "class", "alert alert-danger");
            builder.AddContent(10, _errorMessage);
            builder.CloseElement();
        }
        else if (_forecasts != null)
        {
            // Refresh button
            builder.OpenElement(11, "button");
            builder.AddAttribute(12, "data-testid", "refresh-button");
            builder.AddAttribute(13, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create(this, RefreshData));
            builder.AddContent(14, "Refresh");
            builder.CloseElement();

            // Weather table
            builder.OpenElement(15, "table");
            builder.AddAttribute(16, "class", "table");

            // Header
            builder.OpenElement(17, "thead");
            builder.OpenElement(18, "tr");
            builder.OpenElement(19, "th");
            builder.AddContent(20, "Date");
            builder.CloseElement();
            builder.OpenElement(21, "th");
            builder.AddContent(22, "Temp. (C)");
            builder.CloseElement();
            builder.OpenElement(23, "th");
            builder.AddContent(24, "Summary");
            builder.CloseElement();
            builder.CloseElement();
            builder.CloseElement();

            // Body
            builder.OpenElement(25, "tbody");
            var sequence = 26;
            foreach (var forecast in _forecasts)
            {
                builder.OpenElement(sequence++, "tr");
                builder.OpenElement(sequence++, "td");
                builder.AddContent(sequence++, forecast.Date.ToShortDateString());
                builder.CloseElement();
                builder.OpenElement(sequence++, "td");
                builder.AddContent(sequence++, forecast.TemperatureC.ToString());
                builder.CloseElement();
                builder.OpenElement(sequence++, "td");
                builder.AddContent(sequence++, forecast.Summary);
                builder.CloseElement();
                builder.CloseElement();
            }
            builder.CloseElement();
            builder.CloseElement();
        }

        builder.CloseElement();
    }
}

public interface IWeatherService
{
    Task<IEnumerable<WeatherForecast>> GetForecastAsync(int days);
}

public class WeatherForecast
{
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    public string? Summary { get; set; }
}
