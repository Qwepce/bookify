using Bookify.Application.Abstractions.Authentication;
using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Bookings;

namespace Bookify.Application.Bookings.ConfirmBooking;

public class ConfirmBookingHandler(
    IBookingRepository bookingRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider,
    IUserContext userContext )
    : ICommandHandler<ConfirmBookingCommand>
{
    public async Task<Result> Handle( ConfirmBookingCommand request, CancellationToken cancellationToken )
    {
        Booking? booking = await bookingRepository.GetByIdAsync( request.BookingId, cancellationToken );
        if ( booking is null || booking.UserId != userContext.UserId )
        {
            return Result.Failure( BookingErrors.NotFound );
        }

        Result result = booking.Confirm( dateTimeProvider.UtcNow );
        if ( result.IsFailure )
        {
            return Result.Failure( result.Error );
        }

        await unitOfWork.SaveChangesAsync( cancellationToken );

        return Result.Success();
    }
}