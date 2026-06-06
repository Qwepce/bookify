using Asp.Versioning;
using Bookify.Application.Reviews.AddReview;
using Bookify.Application.Reviews.GetReview;
using Bookify.Domain.Abstractions;
using Bookify.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers.ReviewsController;

[ApiController]
[ApiVersion( ApiVersions.V1 )]
[Route( "api/v{version:apiVersion}/[controller]" )]
public class ReviewsController( ISender sender ) : ControllerBase
{
    [HttpGet( "{reviewId:guid}" )]
    public async Task<IActionResult> GetReview( Guid reviewId, CancellationToken cancellationToken )
    {
        GetReviewQuery query = new( reviewId );

        Result<ReviewResponse> result = await sender.Send( query, cancellationToken );
        if ( result.IsFailure )
        {
            return NotFound();
        }

        return Ok( result.Value );
    }

    [HttpPost]
    [HasPermission( Permissions.UsersRead )]
    public async Task<IActionResult> AddReview(
        AddReviewRequest request,
        CancellationToken cancellationToken )
    {
        AddReviewCommand command = new( request.BookingId, request.Rating, request.Comment );

        Result<Guid> result = await sender.Send( command, cancellationToken );

        if ( result.IsFailure )
        {
            return BadRequest( result.Error );
        }

        return CreatedAtAction( nameof( GetReview ), new
        {
            reviewId = result.Value
        }, result.Value );
    }
}