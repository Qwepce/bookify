using Microsoft.Extensions.Primitives;
using Serilog.Context;

namespace Bookify.Api.Middleware;

public class RequestContextLoggingMiddleware( RequestDelegate next )
{
    private const string CorrelationIdHeaderName = "X-Correlation-Id";

    public Task InvokeAsync( HttpContext httpContext )
    {
        using ( LogContext.PushProperty( "CorrelationId", GetCorrelationId( httpContext ) ) )
        {
            return next( httpContext );
        }
    }

    private static string GetCorrelationId( HttpContext httpContext )
    {
        httpContext.Request.Headers.TryGetValue( CorrelationIdHeaderName, out StringValues correlationId );

        return correlationId.FirstOrDefault() ?? httpContext.TraceIdentifier;
    }
}