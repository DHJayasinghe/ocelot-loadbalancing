using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services
    .AddOcelot(builder.Configuration);
builder.Configuration
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

var app = builder.Build();

app.UseOcelot().Wait();

app.Run();