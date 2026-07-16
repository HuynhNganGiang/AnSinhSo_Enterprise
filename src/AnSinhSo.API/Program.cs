using System;
using System.IO;
using System.Reflection;
using AnSinhSo.API.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // 1. Cấu hình Serilog từ appsettings.json
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();

    builder.Host.UseSerilog();

    Log.Information("Starting AnSinhSo Enterprise API host...");

    // 2. Thêm Controllers
    builder.Services.AddControllers();

    // 3. Cấu hình Health Checks
    builder.Services.AddHealthChecks();

    // 4. Cấu hình Swagger/OpenAPI nâng cao
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "AnSinhSo Enterprise API",
            Description = "Hệ thống REST API cho dự án An Sinh Số xã Sông Lũy.",
            Contact = new OpenApiContact
            {
                Name = "Solution Architecture Team",
                Email = "arch@ansinhso.gov.vn"
            }
        });

        // Cấu hình JWT Placeholder trong Swagger UI
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Nhập token JWT theo định dạng: Bearer {your_token}"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });

        // Tích hợp XML Comments
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
        if (File.Exists(xmlPath))
        {
            options.IncludeXmlComments(xmlPath);
        }
    });

    var app = builder.Build();

    // 5. Cấu hình pipeline HTTP request (Middlewares) theo thứ tự ưu tiên
    // CorrelationIdMiddleware nằm đầu tiên để gán trace ID cho toàn bộ context
    app.UseMiddleware<CorrelationIdMiddleware>();

    // GlobalExceptionMiddleware bao ngoài cùng để bắt toàn bộ lỗi của luồng xử lý
    app.UseMiddleware<GlobalExceptionMiddleware>();

    // Đo lường thời gian thực thi của request
    app.UseMiddleware<RequestTimingMiddleware>();

    // Ghi nhận log truy cập có cấu trúc
    app.UseMiddleware<RequestLoggingMiddleware>();

    // ResponseWrapperMiddleware tự động bọc chuẩn đầu ra
    app.UseMiddleware<ResponseWrapperMiddleware>();

    // Cấu hình Swagger
    if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "AnSinhSo API v1");
            c.RoutePrefix = "swagger"; // Truy cập trực tiếp tại root/swagger
        });
    }

    app.UseHttpsRedirection();

    app.UseRouting();

    app.UseAuthorization();

    // Map các endpoint Controllers và Health Checks
    app.MapControllers();

    app.MapHealthChecks("/health");
    app.MapHealthChecks("/health/live");
    app.MapHealthChecks("/health/ready");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "AnSinhSo API Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
