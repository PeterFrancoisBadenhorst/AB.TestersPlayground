using Microsoft.Playwright;
using Xunit;
using FluentAssertions;

namespace WebUI.FunctionalTests.Pages;

/// <summary>
/// Page Object Model for the main page of both React and Blazor frontends
/// Provides reusable methods for interacting with UI elements
/// </summary>
public class MainPageObject
{
    private readonly IPage _page;

    public MainPageObject(IPage page)
    {
        _page = page;
    }

    // Selectors
    public const string HeaderSelector = "h1";
    public const string ApiTestButtonSelector = "button";
    public const string ApiResultSelector = ".api-result, [data-testid='api-result']";

    // Navigation methods
    public async Task NavigateToAsync(string url)
    {
        await _page.GotoAsync(url);
    }

    // Interaction methods
    public async Task ClickApiTestButtonAsync()
    {
        await _page.ClickAsync(ApiTestButtonSelector);
    }

    public async Task WaitForApiResponseAsync()
    {
        await _page.WaitForSelectorAsync(ApiResultSelector);
    }

    // Assertion helper methods
    public async Task<string> GetHeaderTextAsync()
    {
        return await _page.TextContentAsync(HeaderSelector) ?? string.Empty;
    }

    public async Task<string> GetApiButtonTextAsync()
    {
        return await _page.TextContentAsync(ApiTestButtonSelector) ?? string.Empty;
    }

    public async Task<string> GetApiResultTextAsync()
    {
        return await _page.TextContentAsync(ApiResultSelector) ?? string.Empty;
    }

    public async Task<bool> IsApiButtonDisabledAsync()
    {
        return await _page.IsDisabledAsync(ApiTestButtonSelector);
    }

    public async Task<bool> IsElementVisibleAsync(string selector)
    {
        return await _page.IsVisibleAsync(selector);
    }

    // Screenshot helper
    public async Task TakeScreenshotAsync(string fileName)
    {
        await _page.ScreenshotAsync(new PageScreenshotOptions
        {
            Path = $"screenshots/{fileName}.png",
            FullPage = true
        });
    }
}
