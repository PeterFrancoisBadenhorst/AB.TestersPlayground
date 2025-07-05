using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using frontend_blazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClient for Docker environment 
// Extract the base URL (protocol + host + port) from the current base address
var currentBaseAddress = builder.HostEnvironment.BaseAddress;
var uri = new Uri(currentBaseAddress);
var baseUrl = $"{uri.Scheme}://{uri.Authority}/";

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseUrl) });

await builder.Build().RunAsync();
