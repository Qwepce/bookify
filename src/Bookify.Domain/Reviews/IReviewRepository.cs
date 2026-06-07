namespace Bookify.Domain.Reviews;

public interface IReviewRepository
{
    Task<Review?> GetByIdAsync( Guid reviewId, CancellationToken cancellationToken );

    void Add( Review review );
}