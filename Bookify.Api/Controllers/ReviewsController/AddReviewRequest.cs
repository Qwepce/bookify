namespace Bookify.Api.Controllers.ReviewsController;

public record AddReviewRequest(
    Guid BookingId,
    int Rating,
    string Comment );