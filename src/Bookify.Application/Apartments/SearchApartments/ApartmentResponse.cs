namespace Bookify.Application.Apartments.SearchApartments;

public class ApartmentResponse
{
    public Guid Id { get; init; }

    public string Name { get; init; }

    public string Description { get; init; }

    public decimal PriceAmount { get; init; }

    public string PriceAmountCurrency { get; init; }

    public AddressResponse Address { get; set; }
}