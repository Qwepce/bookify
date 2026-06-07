using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Bookings;

namespace Bookify.Application.Bookings.CancelBooking;

internal sealed class CancelBookingCommandHandler(
    IBookingRepository bookingRepository,
    IDateTimeProvider dateTimeProvider,
    IUnitOfWork unitOfWork )
    : ICommandHandler<CancelBookingCommand>
{
    public async Task<Result> Handle( CancelBookingCommand request, CancellationToken cancellationToken )
    {
        Booking? booking = await bookingRepository.GetByIdAsync( request.BookingId, cancellationToken );
        if ( booking is null )
        {
            return Result.Failure( BookingErrors.NotFound );
        }

        Result result = booking.Cancel( dateTimeProvider.UtcNow );
        if ( result.IsFailure )
        {
            return Result.Failure( result.Error );
        }

        await unitOfWork.SaveChangesAsync( cancellationToken );

        return Result.Success();
    }
}