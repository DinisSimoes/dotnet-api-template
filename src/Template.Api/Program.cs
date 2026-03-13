using OpenTelemetry.Trace;
using Serilog;
using Template.Api.Middlewares;
using Template.Application.DependencyInjection;
using Template.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);


// -------------------------
// Logging (Serilog)
// -------------------------
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();


// -------------------------
// Controllers
// -------------------------
builder.Services.AddControllers();


// -------------------------
// Swagger
// -------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// -------------------------
// Health Checks
// -------------------------
builder.Services.AddHealthChecks();


// -------------------------
// Options Pattern
// -------------------------
builder.Services.ConfigureOptions(builder.Configuration);


// -------------------------
// Dependency Injection Layers
// -------------------------
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);


// -------------------------
// OpenTelemetry (base)
// -------------------------
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing.AddAspNetCoreInstrumentation();
    });

var app = builder.Build();


// -------------------------
// Middleware pipeline
// -------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();