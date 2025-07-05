using NBomber.CSharp;
using NBomber.Http.CSharp;

// NBomber HTTP Load Test for Testers Playground API
var scenario = Scenario.Create("api_load_test", async context =>
{
    var baseUrl = Environment.GetEnvironmentVariable("BASE_URL") ?? "https://localhost:7243";
    
    using var httpClient = new HttpClient();
    
    // Test the weather forecast endpoint
    var weatherResponse = await httpClient.GetAsync($"{baseUrl}/weatherforecast");
    
    if (weatherResponse.IsSuccessStatusCode)
    {
        var content = await weatherResponse.Content.ReadAsStringAsync();
        Console.WriteLine($"Weather forecast received: {content.Length} bytes");
        return Response.Ok();
    }
    
    return Response.Fail(statusCode: $"Failed with status: {weatherResponse.StatusCode}");
})
.WithLoadSimulations(
    Simulation.RampingInject(rate: 5, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromMinutes(1)), // Warm up
    Simulation.RampingInject(rate: 10, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromMinutes(5)), // Main load
    Simulation.RampingInject(rate: 20, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromMinutes(2)), // Peak load
    Simulation.RampingInject(rate: 5, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromMinutes(1))   // Cool down
);

// API Info endpoint test scenario
var infoScenario = Scenario.Create("api_info_test", async context =>
{
    var baseUrl = Environment.GetEnvironmentVariable("BASE_URL") ?? "https://localhost:7243";
    
    using var httpClient = new HttpClient();
    
    var infoResponse = await httpClient.GetAsync($"{baseUrl}/info");
    
    if (infoResponse.IsSuccessStatusCode)
    {
        var content = await infoResponse.Content.ReadAsStringAsync();
        Console.WriteLine($"API info received: {content.Length} bytes");
        return Response.Ok();
    }
    
    return Response.Fail(statusCode: $"Failed with status: {infoResponse.StatusCode}");
})
.WithLoadSimulations(
    Simulation.RampingInject(rate: 2, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromMinutes(9))
);

// Configure and run the load test
NBomberRunner
    .RegisterScenarios(scenario, infoScenario)
    .WithReportFolder("reports")
    .Run();

Console.WriteLine("Load test completed. Check the reports folder for results.");
