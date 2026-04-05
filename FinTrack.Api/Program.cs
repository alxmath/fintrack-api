using FinTrack.Api.Configurations;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithThreadId()
    .Enrich.WithEnvironmentName()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services
    .AddApiServices(builder.Configuration)
    .AddApplicationServices(builder.Configuration)
    .AddJwtAuthentication(builder.Configuration)
    .AddObservability();

var app = builder.Build();

app.UseApiPipeline();

app.Run();
