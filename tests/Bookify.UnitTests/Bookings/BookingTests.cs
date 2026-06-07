using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Bookings.Events;
using Bookify.Domain.Shared;
using Bookify.Domain.Users;
using Bookify.UnitTests.Apartments;
using Bookify.UnitTests.Infrastructure;
using Bookify.UnitTests.Users;
using FluentAssertions;

namespace Bookify.UnitTests.Bookings;

public class BookingTests : BaseTest
{
    [Fact]
    public void Reserve_Should_RaiseBookingReservedDomainEvent()
    {
        // Arrange
        User user = User.Create( UserData.FirstName, UserData.LastName, UserData.Email );
        Money price = new( 10.0m, Currency.Usd );
        DateRange period = DateRange.Create( new DateOnly( 2026, 1, 1 ), new DateOnly( 2026, 1, 10 ) );
        Money expectedTotalPrice = new( price.Amount * period.LengthInDays, price.Currency );
        Apartment apartment = ApartmentData.Create( price );

        PricingService pricingService = new();

        // Act
        Booking booking = Booking.Reserve(
            apartment,
            user.Id,
            period,
            DateTime.UtcNow,
            pricingService );

        // Assert
        BookingReservedDomainEvent domainEvent = AssertDomainEventWasPublished<BookingReservedDomainEvent>( booking );
        domainEvent.BookingId.Should().Be( booking.Id );
    }
}