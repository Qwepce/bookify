using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Reviews;

public static class ReviewErrors
{
    public static Error NotEligable = new(
        "Review.NotEligable",
        "The review is not eligable because booking is not completed yet" );
}