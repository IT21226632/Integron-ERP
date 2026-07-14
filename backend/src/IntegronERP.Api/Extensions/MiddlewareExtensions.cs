using IntegronERP.Api.Middleware;

namespace IntegronERP.Api.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandling(
        this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}