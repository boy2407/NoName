using FluentValidation;
using NoName.Application.Common;
using System.Net;
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

                
                 //context.Response.StatusCode = 500;
                 //context.Response.ContentType = "application/json";
                
                 //var response = ApiResult<string>.Failure("Hệ thống gặp sự cố: " + ex.Message);
                 //await context.Response.WriteAsJsonAsync(response);


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



