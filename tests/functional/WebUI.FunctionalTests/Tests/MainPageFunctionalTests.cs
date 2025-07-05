using Microsoft.Playwright;
using Xunit;
using FluentAssertions;
using WebUI.FunctionalTests.Pages;

namespace WebUI.FunctionalTests.Tests;

/// <summary>
/// End-to-end functional tests for the frontend applications
/// Tests user workflows across both React and Blazor frontends
/// </summary>
public class MainPageFunctionalTests : IAsyncLifetime
{
    private IBrowser? _browser;
    private IPage? _page;
    private MainPageObject? _mainPage;

    public async Task InitializeAsync()
    {
        // Install Playwright browsers if needed
        var playwright = await Playwright.CreateAsync();
        
        // Launch browser with options for testing
        _browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true, // Set to false for debugging
            SlowMo = 50 // Slow down operations for debugging
        });

        _page = await _browser.NewPageAsync();
        _mainPage = new MainPageObject(_page);

        // Create screenshots directory
        Directory.CreateDirectory("screenshots");
    }

    public async Task DisposeAsync()
    {
        if (_page != null)
            await _page.CloseAsync();
        
        if (_browser != null)
            await _browser.CloseAsync();
    }

    [Theory]
    [InlineData("http://localhost:5003", "Blazor")] // Blazor frontend
    [InlineData("http://localhost:3000", "React")]  // React frontend
    public async Task Frontend_LoadsSuccessfully(string url, string frontendType)
    {
        // Act
        await _mainPage!.NavigateToAsync(url);
        await _page!.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        var headerText = await _mainPage.GetHeaderTextAsync();
        headerText.Should().Contain("Testers Playground");
        
        // Take screenshot for verification
        await _mainPage.TakeScreenshotAsync($"{frontendType}_main_page");
    }

    [Theory]
    [InlineData("http://localhost:5003", "Blazor")]
    [InlineData("http://localhost:3000", "React")]
    public async Task ApiTestButton_ExistsAndClickable(string url, string frontendType)
    {
        // Arrange
        await _mainPage!.NavigateToAsync(url);
        await _page!.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Act & Assert
        var buttonText = await _mainPage.GetApiButtonTextAsync();
        buttonText.Should().NotBeNullOrEmpty();
        
        var isVisible = await _mainPage.IsElementVisibleAsync(MainPageObject.ApiTestButtonSelector);
        isVisible.Should().BeTrue($"{frontendType} API test button should be visible");

        // Click the button
        await _mainPage.ClickApiTestButtonAsync();
        
        // Check if button shows loading state
        var isDisabled = await _mainPage.IsApiButtonDisabledAsync();
        isDisabled.Should().BeTrue($"{frontendType} button should be disabled during loading");

        await _mainPage.TakeScreenshotAsync($"{frontendType}_button_clicked");
    }

    [Fact]
    public async Task BlazorFrontend_SpecificFunctionality()
    {
        // Arrange
        const string blazorUrl = "http://localhost:5003";
        await _mainPage!.NavigateToAsync(blazorUrl);
        await _page!.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Act
        var headerText = await _mainPage.GetHeaderTextAsync();

        // Assert
        headerText.Should().Contain("Blazor Frontend");
        
        // Verify Blazor-specific elements exist
        var blazorSpecificContent = await _page.IsVisibleAsync("text=API Connection Test");
        blazorSpecificContent.Should().BeTrue("Blazor should have API Connection Test section");

        await _mainPage.TakeScreenshotAsync("blazor_specific_test");
    }

    [Fact]
    public async Task ReactFrontend_SpecificFunctionality()
    {
        // Arrange
        const string reactUrl = "http://localhost:3000";
        await _mainPage!.NavigateToAsync(reactUrl);
        await _page!.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Act
        var pageTitle = await _page.TitleAsync();

        // Assert
        pageTitle.Should().NotBeNullOrEmpty("React app should have a title");

        // Check for React-specific elements
        var reactApp = await _page.IsVisibleAsync("#root");
        reactApp.Should().BeTrue("React app should have root element");

        await _mainPage.TakeScreenshotAsync("react_specific_test");
    }

    [Theory]
    [InlineData("http://localhost:5003")]
    [InlineData("http://localhost:3000")]
    public async Task Frontend_ResponsiveDesign_Mobile(string url)
    {
        // Arrange - Set mobile viewport
        await _page!.SetViewportSizeAsync(375, 667);
        
        // Act
        await _mainPage!.NavigateToAsync(url);
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Assert
        var headerText = await _mainPage.GetHeaderTextAsync();
        headerText.Should().Contain("Testers Playground");

        // Verify mobile layout
        var isButtonVisible = await _mainPage.IsElementVisibleAsync(MainPageObject.ApiTestButtonSelector);
        isButtonVisible.Should().BeTrue("Button should be visible on mobile");

        var frontendType = url.Contains("5003") ? "blazor" : "react";
        await _mainPage.TakeScreenshotAsync($"{frontendType}_mobile_view");
    }

    [Theory]
    [InlineData("http://localhost:5003")]
    [InlineData("http://localhost:3000")]
    public async Task Frontend_KeyboardNavigation(string url)
    {
        // Arrange
        await _mainPage!.NavigateToAsync(url);
        await _page!.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Act - Navigate using keyboard
        await _page.PressAsync("body", "Tab");
        
        // Assert - Check if button receives focus
        var focusedElement = await _page.EvaluateAsync<string>("document.activeElement.tagName");
        focusedElement.Should().Be("BUTTON", "Button should be focusable via keyboard");

        // Test Enter key activation
        await _page.PressAsync("body", "Enter");
        
        var isDisabled = await _mainPage.IsApiButtonDisabledAsync();
        isDisabled.Should().BeTrue("Button should be activated via Enter key");
    }
}
