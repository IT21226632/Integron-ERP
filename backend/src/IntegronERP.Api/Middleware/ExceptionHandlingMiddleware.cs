using System.Net;
using System.Text.Json;
using FluentValidation;

namespace IntegronERP.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (ValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var response = new
            {
                Success = false,
                Message = "Validation failed.",
                Errors = ex.Errors.Select(x => x.ErrorMessage)
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new
            {
                Success = false,
                Message = "An unexpected error occurred."
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}