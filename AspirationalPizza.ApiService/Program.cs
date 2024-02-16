var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddSwaggerGen();
AspirationalPizza.Library.Configuration.Services.Configure(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

AspirationalPizza.ApiService.Endpoints.Add(app);  //Taking the cognitive load out of adding routes in the main program loop.

app.MapDefaultEndpoints();

//app.UseSwaggerUI();
app.Run();

