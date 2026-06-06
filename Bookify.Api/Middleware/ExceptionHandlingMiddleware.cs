using Bookify.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Middleware;

public class ExceptionHandlingMiddleware( RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger )
{
    public async Task InvokeAsync( HttpContext context )
    {
        try
        {
            await next( context );
        }
        catch ( Exception exception )
        {
            logger.LogError( exception, "Exception occured: {Message}", exception.Message );

            ExceptionDetails exceptionDetails = GetExceptionDetails( exception );

            ProblemDetails problemDetails = new()
            {
                Status = exceptionDetails.Status,
                Type = exceptionDetails.Type,
                Title = exceptionDetails.Title,
                Detail = exceptionDetails.Details,
                Extensions =
                {
                    ["errors"] = exceptionDetails.Errors
                }
            };

            context.Response.StatusCode = exceptionDetails.Status;

            await context.Response.WriteAsJsonAsync( problemDetails );
        }
    }

    private static ExceptionDetails GetExceptionDetails( Exception exception )
    {
        return exception switch
        {
            ValidationException validationException => new ExceptionDetails(
                StatusCodes.Status400BadRequest,
                "ValidationFailure",
                "ValidationError",
                "One or more validation errors has occured",
                validationException.Errors ),
            _ => new ExceptionDetails(
                StatusCodes.Status500InternalServerError,
                "Server error",
                "Server error",
                "An unexpected error has occured",
                null )
        };
    }

    internal record ExceptionDetails(
        int Status,
        string Type,
        string Title,
        string Details,
        IEnumerable<object?> Errors );
}