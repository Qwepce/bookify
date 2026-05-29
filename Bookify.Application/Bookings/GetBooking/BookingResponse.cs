using Bookify.Domain.Bookings;
using Bookify.Domain.Shared;

namespace Bookify.Application.Bookings.GetBooking;

public class BookingResponse
{
    public Guid Id { get; init; }

    public Guid ApartmentId { get; init; }

    public Guid UserId { get; init; }

    public int Status { get; init; }

    public decimal PriceAmount { get; init; }

    public string PriceCurrency { get; init; }

    public decimal CleaningFeeAmount { get; init; }

    public string CleaningFeeAmountCurrency { get; init; }

    public decimal AmenitiesUpChargeAmount { get; init; }

    public string AmenitiesUpChargeAmountCurrency { get; init; }

    public decimal TotalPriceAmount { get; init; }

    public string TotalPriceCurrency { get; init; }

    public DateOnly DurationStart { get; init; }

    public DateOnly DurationEnd { get; init; }

    public DateTime ConfirmedOnUtc { get; init; }
}