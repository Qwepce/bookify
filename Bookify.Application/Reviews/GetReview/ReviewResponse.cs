namespace Bookify.Application.Reviews.GetReview;

public record ReviewResponse(
    Guid ApartmentId,
    Guid UserId,
    int Rating,
    string Comment );