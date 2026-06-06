using Bookify.Application.Abstractions.Authentication;
using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Bookings;
using Bookify.Domain.Reviews;

namespace Bookify.Application.Reviews.AddReview;

public class AddReviewCommandHandler(
    IBookingRepository bookingRepository,
    IReviewRepository reviewRepository,
    IDateTimeProvider dateTimeProvider,
    IUserContext userContext,
    IUnitOfWork unitOfWork )
    : ICommandHandler<AddReviewCommand, Guid>
{
    public async Task<Result<Guid>> Handle( AddReviewCommand request, CancellationToken cancellationToken )
    {
        Booking? booking = await bookingRepository.GetByIdAsync( request.BookingId, cancellationToken );
        if ( booking is null || booking.UserId != userContext.UserId )
        {
            return Result.Failure<Guid>( BookingErrors.NotFound );
        }

        Result<Rating> ratingResult = Rating.Create( request.Rating );
        if ( ratingResult.IsFailure )
        {
            return Result.Failure<Guid>( ratingResult.Error );
        }

        Result<Review> reviewResult = Review.Create(
            booking,
            ratingResult.Value,
            new Comment( request.Comment ),
            dateTimeProvider.UtcNow );

        if ( reviewResult.IsFailure )
        {
            return Result.Failure<Guid>( reviewResult.Error );
        }

        reviewRepository.Add( reviewResult.Value );

        await unitOfWork.SaveChangesAsync( cancellationToken );

        return Result.Success( reviewResult.Value.Id );
    }
}