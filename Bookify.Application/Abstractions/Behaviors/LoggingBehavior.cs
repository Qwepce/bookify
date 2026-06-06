using Bookify.Domain.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Bookify.Application.Abstractions.Behaviors;

public class LoggingBehavior<TRequest, TResponse>( ILogger<LoggingBehavior<TRequest, TResponse>> logger )
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseRequest
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken )
    {
        string name = request.GetType().Name;

        try
        {
            logger.LogInformation( "Executing request {Request}", name );

            TResponse result = await next( cancellationToken );

            if ( result.IsSuccess )
            {
                logger.LogInformation( "Request {Request} processed successfully", name );
            }
            else
            {
                using ( LogContext.PushProperty( "Error", result.Error, false ) )
                {
                    logger.LogError( "Request {Request} with error", name );
                }
            }

            return result;
        }
        catch ( Exception ex )
        {
            logger.LogError( ex, "Request {Request} processing failed", name );

            throw;
        }
    }
}