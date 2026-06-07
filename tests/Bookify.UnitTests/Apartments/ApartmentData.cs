using Bookify.Domain.Apartments;
using Bookify.Domain.Shared;

namespace Bookify.UnitTests.Apartments;

internal static class ApartmentData
{
    public static Apartment Create( Money price, Money? cleaningFee = null ) => new(
        Guid.CreateVersion7(),
        new Name( "Test apartment" ),
        new Description( "Test apartment description" ),
        new Address( "Country", "State", "ZipCode", "City", "Street" ),
        price,
        cleaningFee ?? Money.Zero(),
        []
    );
}