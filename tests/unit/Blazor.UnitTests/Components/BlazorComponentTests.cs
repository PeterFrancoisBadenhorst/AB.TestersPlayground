using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Blazor.UnitTests.Components;

/// <summary>
/// Tests for Blazor frontend components and functionality
/// </summary>
public class BlazorComponentTests : TestContext
{
    [Fact]
    public void TestContext_ShouldBeSetupCorrectly()
    {
        // Arrange & Act
        var services = Services;
        
        // Assert
        services.Should().NotBeNull();
    }

    [Fact]
    public void HttpClient_ShouldBeConfigurable()
    {
        // Arrange
        Services.AddHttpClient();
        
        // Act
        var httpClient = Services.GetService<HttpClient>();
        
        // Assert
        httpClient.Should().NotBeNull();
    }

    [Fact]
    public void Services_ShouldAllowCustomConfiguration()
    {
        // Arrange
        Services.AddScoped<ITestService, TestService>();
        
        // Act
        var service = Services.GetService<ITestService>();
        
        // Assert
        service.Should().NotBeNull();
        service.Should().BeOfType<TestService>();
    }

    [Fact]
    public void Components_ShouldRenderBasicMarkup()
    {
        // Arrange
        var component = RenderComponent<TestComponent>();
        
        // Act
        var markup = component.Markup;
        
        // Assert
        markup.Should().NotBeNullOrEmpty();
        markup.Should().Contain("Test Component");
    }

    [Fact]
    public void Components_ShouldSupportParameterBinding()
    {
        // Arrange
        var testValue = "Test Value";
        
        // Act
        var component = RenderComponent<TestComponent>(parameters => parameters
            .Add(p => p.Title, testValue));
        
        // Assert
        var markup = component.Markup;
        markup.Should().Contain(testValue);
    }

    [Fact]
    public void Components_ShouldHandleEvents()
    {
        // Arrange
        var component = RenderComponent<TestComponent>();
        var button = component.Find("button");
        
        // Act
        button.Click();
        
        // Assert
        var markup = component.Markup;
        markup.Should().Contain("Button clicked");
    }
}

// Test interfaces and components for unit testing
public interface ITestService
{
    string GetData();
}

public class TestService : ITestService
{
    public string GetData() => "Test Data";
}

// Simple test component for Blazor testing
public class TestComponent : ComponentBase
{
    [Parameter] public string Title { get; set; } = "Test Component";
    
    private bool _clicked = false;
    
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "class", "test-component");
        
        builder.OpenElement(2, "h1");
        builder.AddContent(3, Title);
        builder.CloseElement();
        
        builder.OpenElement(4, "button");
        builder.AddAttribute(5, "onclick", EventCallback.Factory.Create(this, HandleClick));
        builder.AddContent(6, _clicked ? "Button clicked" : "Click me");
        builder.CloseElement();
        
        builder.CloseElement();
    }
    
    private void HandleClick()
    {
        _clicked = true;
        StateHasChanged();
    }
}
