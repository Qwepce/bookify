using Bookify.Application.Cache;

namespace Bookify.Application.Reviews.GetReview;

public record GetReviewQuery( Guid ReviewId ) : ICachedQuery<ReviewResponse>
{
    public string CacheKey => $"reviews-{ReviewId}";
    public TimeSpan? Expiration => null;
}