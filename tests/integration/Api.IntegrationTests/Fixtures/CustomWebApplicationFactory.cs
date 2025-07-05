using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Testcontainers.PostgreSql;
using Xunit;

namespace Api.IntegrationTests.Fixtures;

/// <summary>
/// Custom WebApplicationFactory for integration tests
/// Provides test-specific configuration and database isolation
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private PostgreSqlContainer? _dbContainer;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove existing database context if any
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContext));
            if (descriptor != null)
                services.Remove(descriptor);

            // Add test-specific services
            services.AddLogging(logging => logging.AddConsole());
            
            // Configure test database connection
            if (_dbContainer != null)
            {
                var connectionString = _dbContainer.GetConnectionString();
                // Add your database context with test connection string
                // services.AddDbContext<YourDbContext>(options => options.UseNpgsql(connectionString));
            }
        });

        // Configure test environment
        builder.UseEnvironment("Testing");
    }

    public async Task InitializeAsync()
    {
        // Start PostgreSQL container for tests
        _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:15")
            .WithDatabase("testdb")
            .WithUsername("testuser")
            .WithPassword("testpass")
            .WithCleanUp(true)
            .Build();

        await _dbContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        if (_dbContainer != null)
        {
            await _dbContainer.DisposeAsync();
        }
        await base.DisposeAsync();
    }
}

/// <summary>
/// Placeholder DbContext for demonstration
/// In a real application, this would be your actual DbContext
/// </summary>
public class DbContext
{
    // Database context implementation would go here
}
