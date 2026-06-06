namespace Bookify.Application.Bookings.GetBooking;

public class BookingResponse
{
    public Guid Id { get; init; }

    public Guid ApartmentId { get; init; }

    public Guid UserId { get; init; }

    public int Status { get; init; }

    public decimal PriceForPeriodAmount { get; init; }

    public string PriceForPeriodAmountCurrency { get; init; } = string.Empty;

    public decimal CleaningFeeAmount { get; init; }

    public string CleaningFeeAmountCurrency { get; init; } = string.Empty;

    public decimal AmenitiesUpChargeAmount { get; init; }

    public string AmenitiesUpChargeAmountCurrency { get; init; } = string.Empty;

    public decimal TotalPriceAmount { get; init; }

    public string TotalPriceAmountCurrency { get; init; } = string.Empty;

    public DateOnly DurationStart { get; init; }

    public DateOnly DurationEnd { get; init; }

    public DateTime CreatedOnUtc { get; init; }

    public DateTime ConfirmedOnUtc { get; init; }

    public DateTime CompletedOnUtc { get; init; }
}