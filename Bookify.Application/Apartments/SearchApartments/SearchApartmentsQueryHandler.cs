using Bookify.Application.Abstractions.Data;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Bookings;
using Dapper;

namespace Bookify.Application.Apartments.SearchApartments;

internal sealed class SearchApartmentsQueryHandler
    : IQueryHandler<SearchApartmentsQuery, IReadOnlyList<ApartmentResponse>>
{
    private static readonly int[] ActiveBookingStatuses =
    {
        (int)BookingStatus.Reserved,
        (int)BookingStatus.Confirmed,
        (int)BookingStatus.Completed
    };
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public SearchApartmentsQueryHandler( ISqlConnectionFactory sqlConnectionFactory )
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<IReadOnlyList<ApartmentResponse>>> Handle( SearchApartmentsQuery request, CancellationToken cancellationToken )
    {
        if ( request.StartDate > request.EndDate )
        {
            return new List<ApartmentResponse>();
        }

        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = """
            SELECT
                a.id as Id,
                a.name as Name,
                a.description as Description,
                a.price_amount as PriceAmount,
                a.price_amount_currency as PriceAmountCurrency,
                a.address_country as Country,
                a.address_state as State,
                a.address_zip_code as ZipCode,
                a.address_city as City,
                a.address_street as Street
            FROM apartments AS a
            WHERE NOT EXISTS
            (
                SELECT 1
                FROM bookings as b
                WHERE
                    b.apartment_id = a.id AND
                    b.duration_start <= @EndDate AND
                    b.duration_end >= @StartDate AND
                    b.status = ANY(@ActiveBookingStatuses)
            )
            """;

        var apartments = await connection
            .QueryAsync<ApartmentResponse, AddressResponse, ApartmentResponse>(
            sql,
            ( apartment, address ) =>
            {
                apartment.Address = address;

                return apartment;
            },
            new
            {
                request.StartDate,
                request.EndDate,
                ActiveBookingStatuses
            },
            splitOn: "Country" );

        return apartments.ToList();
    }
}
