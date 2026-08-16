using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AnSinhSo.Api.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        Console.WriteLine("GLOBAL EXCEPTION: " + exception.ToString());
        
        if (context.Response.HasStarted)
        {
            Console.WriteLine("Response has already started, unable to write exception details.");
            return;
        }

        context.Response.ContentType = "application/json";

        var statusCode = StatusCodes.Status500InternalServerError;
        var message = "An error occurred while processing your request.";

        switch (exception)
        {
            case UnauthorizedAccessException _:
                statusCode = StatusCodes.Status401Unauthorized;
                message = "Unauthorized access.";
                break;
            case KeyNotFoundException _:
                statusCode = StatusCodes.Status404NotFound;
                message = "Resource not found.";
                break;
            case Exception _:
                statusCode = StatusCodes.Status500InternalServerError;
                message = exception.Message;
                break;
        }

        context.Response.StatusCode = statusCode;

        var result = new
        {
            success = false,
            message = message,
            statusCode = statusCode
        };

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(result, options));
    }
}
