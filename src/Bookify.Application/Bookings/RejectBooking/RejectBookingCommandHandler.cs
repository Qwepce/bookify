using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Bookings;

namespace Bookify.Application.Bookings.RejectBooking;

internal sealed class RejectBookingCommandHandler(
    IBookingRepository bookingRepository,
    IDateTimeProvider dateTimeProvider,
    IUnitOfWork unitOfWork )
    : ICommandHandler<RejectBookingCommand>
{
    public async Task<Result> Handle( RejectBookingCommand request, CancellationToken cancellationToken )
    {
        Booking? booking = await bookingRepository.GetByIdAsync( request.BookingId, cancellationToken );
        if ( booking is null )
        {
            return Result.Failure( BookingErrors.NotFound );
        }

        Result result = booking.Reject( dateTimeProvider.UtcNow );
        if ( result.IsFailure )
        {
            return Result.Failure( result.Error );
        }

        await unitOfWork.SaveChangesAsync( cancellationToken );

        return Result.Success();
    }
}