var builder = DistributedApplication.CreateBuilder(args);

// Add PostgreSQL database
var postgres = builder.AddPostgres("postgres")
    .WithEnvironment("POSTGRES_DB", "testersplayground")
    .WithEnvironment("POSTGRES_USER", "postgres")
    .WithEnvironment("POSTGRES_PASSWORD", "password");

var database = postgres.AddDatabase("testersplayground");

// Add API project
var api = builder.AddProject<Projects.Api>("api")
    .WithReference(database)
    .WithExternalHttpEndpoints();

// Add Blazor Frontend as container (since it's a WebAssembly app)
var blazorApp = builder.AddDockerfile("frontend-blazor", "../frontend-blazor")
    .WithHttpEndpoint(port: 5003, targetPort: 80)
    .WithExternalHttpEndpoints();

// Add React Frontend as container (since we have Dockerfile)
var reactApp = builder.AddDockerfile("frontend-react", "../frontend-react")
    .WithHttpEndpoint(port: 3000, targetPort: 80)
    .WithExternalHttpEndpoints();

builder.Build().Run();
