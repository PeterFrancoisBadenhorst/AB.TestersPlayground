using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Blazor.UnitTests.Pages;

/// <summary>
/// Unit tests for page components using bUnit
/// Demonstrates testing page rendering, interactions, and lifecycle
/// </summary>
public class HomePageTests : TestContext
{
    [Fact]
    public void HomePage_WhenRendered_ShouldDisplayWelcomeMessage()
    {
        // Arrange
        var component = RenderComponent<HomePage>();

        // Act & Assert
        component.Find("h1").TextContent.Should().Contain("Welcome");
        component.Find("p").TextContent.Should().Contain("Hello, world!");
    }

    [Fact]
    public void HomePage_WhenCounterButtonClicked_ShouldIncrementCounter()
    {
        // Arrange
        var component = RenderComponent<HomePage>();
        var button = component.Find("button");
        var counterDisplay = component.Find("[data-testid='counter']");

        // Act
        button.Click();

        // Assert
        counterDisplay.TextContent.Should().Contain("1");
    }

    [Fact]
    public void HomePage_WhenClickedMultipleTimes_ShouldIncrementCorrectly()
    {
        // Arrange
        var component = RenderComponent<HomePage>();
        var button = component.Find("button");
        var counterDisplay = component.Find("[data-testid='counter']");

        // Act
        button.Click();
        button.Click();
        button.Click();

        // Assert
        counterDisplay.TextContent.Should().Contain("3");
    }

    [Fact]
    public void HomePage_InitialState_ShouldShowZeroCounter()
    {
        // Arrange & Act
        var component = RenderComponent<HomePage>();
        var counterDisplay = component.Find("[data-testid='counter']");

        // Assert
        counterDisplay.TextContent.Should().Contain("0");
    }
}

/// <summary>
/// Sample home page component for testing
/// In a real application, this would reference actual page components
/// </summary>
public partial class HomePage : Microsoft.AspNetCore.Components.ComponentBase
{
    private int _counter = 0;

    private void IncrementCounter()
    {
        _counter++;
    }

    protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "class", "page");

        // Header
        builder.OpenElement(2, "h1");
        builder.AddContent(3, "Welcome to Blazor Test App");
        builder.CloseElement();

        // Welcome message
        builder.OpenElement(4, "p");
        builder.AddContent(5, "Hello, world! Welcome to your new app.");
        builder.CloseElement();

        // Counter section
        builder.OpenElement(6, "div");
        builder.AddAttribute(7, "class", "counter-section");

        builder.OpenElement(8, "p");
        builder.AddAttribute(9, "data-testid", "counter");
        builder.AddContent(10, $"Current count: {_counter}");
        builder.CloseElement();

        builder.OpenElement(11, "button");
        builder.AddAttribute(12, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create(this, IncrementCounter));
        builder.AddContent(13, "Click me");
        builder.CloseElement();

        builder.CloseElement(); // counter-section
        builder.CloseElement(); // page
    }
}
