using System.Data;
using Bookify.Application.Abstractions.Data;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Reviews;
using Dapper;

namespace Bookify.Application.Reviews.GetReview;

internal sealed class GetReviewQueryHandler( ISqlConnectionFactory sqlConnectionFactory )
    : IQueryHandler<GetReviewQuery, ReviewResponse>
{
    public async Task<Result<ReviewResponse>> Handle( GetReviewQuery request, CancellationToken cancellationToken )
    {
        using IDbConnection connection = sqlConnectionFactory.CreateConnection();

        const string sql = """
                           SELECT 
                                apartment_id AS ApartmentId,
                                user_id as UserId,
                                rating as Rating,
                                comment as Comment
                           FROM reviews
                           WHERE id = @ReviewId
                           """;

        var result = await connection.QueryFirstOrDefaultAsync<ReviewResponse>(
            sql,
            new
            {
                request.ReviewId
            } );

        if ( result is null )
        {
            return Result.Failure<ReviewResponse>( ReviewErrors.NotFound );
        }

        return result;
    }
}