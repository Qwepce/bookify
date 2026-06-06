using Bookify.Application.Abstractions.Authentication;
using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Cache;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Bookings;

namespace Bookify.Application.Bookings.CompleteBooking;

internal sealed class CompleteBookingCommandHandler(
    IBookingRepository bookingRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider,
    IUserContext userContext,
    ICacheService cacheService )
    : ICommandHandler<CompleteBookingCommand>
{
    public async Task<Result> Handle( CompleteBookingCommand request, CancellationToken cancellationToken )
    {
        Booking? booking = await bookingRepository.GetByIdAsync( request.BookingId, cancellationToken );
        if ( booking is null || booking.UserId != userContext.UserId )
        {
            return Result.Failure( BookingErrors.NotFound );
        }

        Result result = booking.Complete( dateTimeProvider.UtcNow );
        if ( result.IsFailure )
        {
            return Result.Failure( result.Error );
        }

        string cachedKey = $"bookings-{booking.Id}";
        await cacheService.RemoveAsync( cachedKey, cancellationToken );

        await unitOfWork.SaveChangesAsync( cancellationToken );

        return Result.Success();
    }
}