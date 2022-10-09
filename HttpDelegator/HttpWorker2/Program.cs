var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapGet("/echo", () =>
{
    return "Hello from worker #1";
});

app.Run();