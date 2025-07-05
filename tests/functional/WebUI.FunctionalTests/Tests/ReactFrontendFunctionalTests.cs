using Microsoft.Playwright;
using FluentAssertions;
using Xunit;

namespace WebUI.FunctionalTests.Tests;

/// <summary>
/// Functional tests for React frontend
/// Tests user interactions and UI behavior
/// </summary>
public class ReactFrontendFunctionalTests : IAsyncLifetime
{
    private IPlaywright _playwright = null!;
    private IBrowser _browser = null!;
    private IPage _page = null!;

    public async Task InitializeAsync()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true // Set to false for debugging
        });
        _page = await _browser.NewPageAsync();
    }

    public async Task DisposeAsync()
    {
        if (_page != null)
            await _page.CloseAsync();
        if (_browser != null)
            await _browser.CloseAsync();
        _playwright?.Dispose();
    }

    [Fact]
    public async Task ReactApp_ShouldLoadSuccessfully()
    {
        // Act
        await _page.GotoAsync("http://localhost:3000"); // React dev server default port
        
        // Assert
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        var title = await _page.TitleAsync();
        title.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task ReactApp_ShouldDisplayMainContent()
    {
        // Arrange
        await _page.GotoAsync("http://localhost:3000");
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Act & Assert
        var content = await _page.TextContentAsync("body");
        content.Should().NotBeNullOrEmpty();
        
        // Check for common React app elements
        var appElement = await _page.QuerySelectorAsync("#root");
        appElement.Should().NotBeNull();
    }

    [Fact]
    public async Task ReactApp_ShouldHandleNavigation()
    {
        // Arrange
        await _page.GotoAsync("http://localhost:3000");
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Act - Try to find and click navigation elements
        var navLinks = await _page.QuerySelectorAllAsync("a, button[data-testid*='nav']");
        
        // Assert
        if (navLinks.Any())
        {
            // Click first navigation element
            await navLinks.First().ClickAsync();
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            
            // Verify page responded to navigation
            var currentUrl = _page.Url;
            currentUrl.Should().NotBeNullOrEmpty();
        }
    }

    [Fact]
    public async Task ReactApp_ShouldLoadWithoutConsoleErrors()
    {
        // Arrange
        var consoleMessages = new List<string>();
        _page.Console += (_, e) =>
        {
            if (e.Type == "error")
            {
                consoleMessages.Add(e.Text);
            }
        };

        // Act
        await _page.GotoAsync("http://localhost:3000");
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        
        // Wait a bit for any async errors
        await Task.Delay(2000);

        // Assert
        consoleMessages.Should().BeEmpty($"Console errors found: {string.Join(", ", consoleMessages)}");
    }

    [Fact]
    public async Task ReactApp_ShouldBeResponsive()
    {
        // Arrange
        await _page.GotoAsync("http://localhost:3000");
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Act & Assert - Test different viewport sizes
        var viewportSizes = new[]
        {
            new { Width = 1920, Height = 1080 }, // Desktop
            new { Width = 768, Height = 1024 },  // Tablet
            new { Width = 375, Height = 667 }    // Mobile
        };

        foreach (var size in viewportSizes)
        {
            await _page.SetViewportSizeAsync(size.Width, size.Height);
            await Task.Delay(500); // Allow time for responsive changes
            
            // Verify page is still functional
            var bodyElement = await _page.QuerySelectorAsync("body");
            bodyElement.Should().NotBeNull();
            
            var isVisible = await bodyElement!.IsVisibleAsync();
            isVisible.Should().BeTrue($"Page should be visible at {size.Width}x{size.Height}");
        }
    }

    [Fact]
    public async Task ReactApp_ShouldHandleAPIInteractions()
    {
        // Arrange
        await _page.GotoAsync("http://localhost:3000");
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Act - Look for elements that might trigger API calls
        var buttons = await _page.QuerySelectorAllAsync("button:not([disabled])");
        var loadingElements = new List<IElementHandle>();

        // Try clicking buttons and check for loading states
        foreach (var button in buttons.Take(3)) // Test first 3 buttons only
        {
            try
            {
                await button.ClickAsync();
                await Task.Delay(1000); // Allow time for API call
                
                // Look for loading indicators
                var loadingSpinner = await _page.QuerySelectorAsync("[data-testid*='loading'], .loading, .spinner");
                if (loadingSpinner != null)
                {
                    loadingElements.Add(loadingSpinner);
                }
            }
            catch
            {
                // Some buttons might not be clickable, skip them
            }
        }

        // Assert - If we found loading states, verify they disappear
        foreach (var loading in loadingElements)
        {
            try
            {
                await loading.WaitForElementStateAsync(ElementState.Hidden, new() { Timeout = 10000 });
            }
            catch (TimeoutException)
            {
                // Loading element didn't disappear - might indicate an issue
                var isVisible = await loading.IsVisibleAsync();
                isVisible.Should().BeFalse("Loading indicators should disappear after API calls complete");
            }
        }
    }
}
