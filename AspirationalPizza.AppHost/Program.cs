var builder = DistributedApplication.CreateBuilder(args);

var apiservice = builder.AddProject<Projects.AspirationalPizza_ApiService>("apiservice");

builder.AddProject<Projects.AspirationalPizza_Web>("webfrontend")
    .WithReference(apiservice);

builder.Build().Run();
