using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Http;
using NBomber.Http.CSharp;

// Define a step to fetch weather forecast
var weatherForecastStep = Step.Create("Get Weather Forecast", async context =>
{
    // Create HTTP client
    var client = HttpClientFactory.Create();
    
    // Create and execute HTTP request
    var response = await client.GetAsync("http://nginx/api/weatherforecast", context);

    // Return appropriate response based on status code
    return response.IsSuccessStatusCode
        ? Response.Ok(
            statusCode: (int)response.StatusCode, 
            sizeBytes: (int)(response.ContentLength ?? 0),
            payload: await response.Content.ReadAsStringAsync())
        : Response.Fail(
            statusCode: (int)response.StatusCode);
});

// Create a scenario with the step
var scenario = Scenario.Create("API Load Test", weatherForecastStep)
    .WithWarmUpDuration(TimeSpan.FromSeconds(5))
    .WithLoadSimulations(
        Simulation.Inject(rate: 50, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromSeconds(30)),
        Simulation.Inject(rate: 100, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromMinutes(1))
    );

// Run the test
NBomberRunner
    .RegisterScenarios(scenario)
    .WithTestName("Weather Forecast Endpoint Test")
    .WithReportFileName("nbomber-report")
    .Run();
