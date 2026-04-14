using FluentValidation;
using NoName.Application.Common;
using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace NoName.BackendApi
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

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // Default 500
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var result = "";

            if (exception is ValidationException validationException)
            {
                // If validation error, return 400 with details
                statusCode = (int)HttpStatusCode.BadRequest;
                var errors = validationException.Errors.Select(e => e.ErrorMessage);
                result = JsonSerializer.Serialize(new { errors });
            }
            else if (exception is DbUpdateException dbException)
            {
                // Database update exception - provide more detailed error
                statusCode = (int)HttpStatusCode.BadRequest;
                var innerMessage = dbException.InnerException?.Message ?? dbException.Message;
                result = JsonSerializer.Serialize(new 
                { 
                    error = "Database error occurred.",
                    details = innerMessage
                });
            }
            else
            {
                result = JsonSerializer.Serialize(new { error = exception.Message });
            }

            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(result);
        }
    }
}



