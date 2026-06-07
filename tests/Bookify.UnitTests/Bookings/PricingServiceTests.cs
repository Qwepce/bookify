using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Shared;
using Bookify.UnitTests.Apartments;
using FluentAssertions;

namespace Bookify.UnitTests.Bookings;

public class PricingServiceTests
{
    [Fact]
    public void CalculatePrice_Should_ReturnCorrectTotalPrice()
    {
        // Arrange
        Money price = new( 10.0m, Currency.Usd );
        DateRange period = DateRange.Create( new DateOnly( 2026, 1, 1 ), new DateOnly( 2026, 1, 10 ) );
        Money expectedTotalPrice = new( price.Amount * period.LengthInDays, price.Currency );
        Apartment apartment = ApartmentData.Create( price );

        PricingService pricingService = new();

        // Act
        PricingDetails result = pricingService.CalculatePrice( apartment, period );

        // Assert
        result.TotalPrice.Should().Be( expectedTotalPrice );
    }

    [Fact]
    public void CalculatePrice_Should_ReturnCorrectTotalPrice_WhenCleaningFeeIsIncluded()
    {
        // Arrange
        Money price = new( 10.0m, Currency.Usd );
        Money cleaningFee = new( 99.9m, Currency.Usd );
        DateRange period = DateRange.Create( new DateOnly( 2026, 1, 1 ), new DateOnly( 2026, 1, 10 ) );
        Money expectedTotalPrice = new( price.Amount * period.LengthInDays + cleaningFee.Amount, price.Currency );
        Apartment apartment = ApartmentData.Create( price, cleaningFee );

        PricingService pricingService = new();

        // Act
        PricingDetails result = pricingService.CalculatePrice( apartment, period );

        // Assert
        result.TotalPrice.Should().Be( expectedTotalPrice );
    }
}