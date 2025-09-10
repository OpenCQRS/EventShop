var builder = DistributedApplication.CreateBuilder(args);

// Add the Blazor Server project
var server = builder.AddProject<Projects.EventShop_Web_Server>("server")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development");

// Add the Blazor WebAssembly client project 
var client = builder.AddProject<Projects.EventShop_Web_Client>("client")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development");

var app = builder.Build();

app.Run();
