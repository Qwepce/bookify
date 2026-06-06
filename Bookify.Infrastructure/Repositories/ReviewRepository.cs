using Bookify.Domain.Reviews;

namespace Bookify.Infrastructure.Repositories;

public sealed class ReviewRepository : Repository<Review>, IReviewRepository
{
    public ReviewRepository( ApplicationDbContext context )
        : base( context )
    {
    }
}