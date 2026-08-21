using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace AnSinhSo.Api.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerGenWithAuth(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "AnSinhSo Enterprise API",
                Version = "1.0",
                Description = @"# AnSinhSo Enterprise API
**Hệ thống Quản lý An sinh số xã Sông Lũy**

Phiên bản: 1.0

**Công nghệ:**
* ASP.NET Core 8
* Clean Architecture
* Domain Driven Design
* CQRS + MediatR
* Entity Framework Core
* SQL Server
* JWT Authentication
* GIS
* Explainable AI

Tài liệu này mô tả toàn bộ RESTful API của hệ thống.
"
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
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
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new System.Collections.Generic.List<string>()
                }
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }

            var contractsXmlFile = "AnSinhSo.Contracts.xml";
            var contractsXmlPath = Path.Combine(AppContext.BaseDirectory, contractsXmlFile);
            if (File.Exists(contractsXmlPath))
            {
                options.IncludeXmlComments(contractsXmlPath);
            }

            options.DocumentFilter<OrderTagsDocumentFilter>();
            options.SchemaFilter<EnumSchemaFilter>();
        });

        return services;
    }
}
