using AnSinhSo.Application;
using AnSinhSo.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using AnSinhSo.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(context.Configuration);
});

// Add services to the container.
builder.Services.AddControllers();

// Configure Basic Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Basic Health Checks
builder.Services.AddHealthChecks();

// Register Layer Dependencies
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddApiAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<AnSinhSo.API.Middlewares.CorrelationIdMiddleware>();
app.UseMiddleware<AnSinhSo.API.Middlewares.RequestLoggingMiddleware>();
app.UseMiddleware<AnSinhSo.API.Middlewares.RequestTimingMiddleware>();
app.UseMiddleware<AnSinhSo.Api.Middleware.GlobalExceptionMiddleware>();
app.UseMiddleware<AnSinhSo.API.Middlewares.SecurityHeadersMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<AnSinhSo.API.Middlewares.ResponseWrapperMiddleware>();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();
