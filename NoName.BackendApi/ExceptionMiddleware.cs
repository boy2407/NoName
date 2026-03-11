using System.Net;
using FluentValidation;
using System.Text.Json;

namespace NoName.BackendApi
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
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

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
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
            else
            {
                result = JsonSerializer.Serialize(new { error = exception.Message });
            }

            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(result);
        }
    }
}



