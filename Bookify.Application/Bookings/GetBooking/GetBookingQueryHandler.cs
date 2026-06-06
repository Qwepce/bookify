using Bookify.Application.Abstractions.Authentication;
using Bookify.Application.Abstractions.Data;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Bookings;
using Dapper;

namespace Bookify.Application.Bookings.GetBooking;

internal sealed class GetBookingQueryHandler : IQueryHandler<GetBookingQuery, BookingResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IUserContext _userContext;

    public GetBookingQueryHandler( ISqlConnectionFactory connectionFactory, IUserContext userContext )
    {
        _sqlConnectionFactory = connectionFactory;
        _userContext = userContext;
    }

    public async Task<Result<BookingResponse>> Handle( GetBookingQuery request, CancellationToken cancellationToken )
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = """
            SELECT
                id as Id,
                apartment_id as ApartmentId,
                user_id as UserId,
                status as Status,
                price_for_period_amount as PriceForPeriodAmount,
                price_for_period_currency as PriceForPeriodAmountCurrency,
                cleaning_fee_amount as CleaningFeeAmount,
                cleaning_fee_currency as CleaningFeeAmountCurrency,
                amenities_up_charge_amount as AmenitiesUpChargeAmount,
                amenities_up_charge_currency as AmenitiesUpChargeAmountCurrency,
                total_price_amount as TotalPriceAmount,
                total_price_currency as TotalPriceAmountCurrency,
                duration_start as DurationStart,
                duration_end as DurationEnd,
                created_on_utc as CreatedOnUtc,
                confirmed_on_utc as ConfirmedOnUtc,
                completed_on_utc as CompletedOnUtc
            FROM bookings
            WHERE id = @BookingId
            """;

        var booking = await connection.QueryFirstOrDefaultAsync<BookingResponse>(
            sql,
            new
            {
                request.BookingId
            } );

        if ( booking is null || booking.UserId != _userContext.UserId )
        {
            return Result.Failure<BookingResponse>( BookingErrors.NotFound );
        }

        return booking;
    }
}
