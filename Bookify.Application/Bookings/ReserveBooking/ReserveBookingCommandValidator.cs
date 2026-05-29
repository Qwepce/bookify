using FluentValidation;

namespace Bookify.Application.Bookings.ReserveBooking;

public class ReserveBookingCommandValidator : AbstractValidator<ReserverBookingCommand>
{
    public ReserveBookingCommandValidator()
    {
        RuleFor( c => c.UserId ).NotEmpty();

        RuleFor( c => c.ApartmentId ).NotEmpty();

        RuleFor( c => c.StartDate ).LessThan( c => c.EndDate );
    }
}
