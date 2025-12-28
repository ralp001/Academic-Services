using Academic.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Academic.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext); // Execute the rest of the pipeline
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred during request processing.");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError; // Default to 500

            // Define the custom error payload
            var errorDetails = new
            {
                message = "An error occurred.",
                detailedMessage = exception.Message,
                code = statusCode
            };

            // Map custom exceptions to specific HTTP status codes
            if (exception is NotFoundException)
            {
                statusCode = HttpStatusCode.NotFound; // 404
                errorDetails = new
                {
                    message = "Resource Not Found",
                    detailedMessage = exception.Message,
                    code = statusCode
                };
            }
            // Future additions: Catching ValidationException (400), UnauthorizedException (401), etc.

            context.Response.StatusCode = (int)statusCode;
            var result = JsonSerializer.Serialize(errorDetails);

            return context.Response.WriteAsync(result);
        }
    }
}