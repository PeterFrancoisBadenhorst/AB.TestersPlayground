using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;

namespace Api.UnitTests.Helpers;

/// <summary>
/// Custom Web Application Factory for testing
/// Provides isolated test environment with customized configuration
/// </summary>
public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the app's database registration if any
            // Add test-specific services here
        });

        builder.UseEnvironment("Testing");
        
        // Suppress logging during tests
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
        });
    }
}
