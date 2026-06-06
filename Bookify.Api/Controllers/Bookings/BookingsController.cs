using Asp.Versioning;
using Bookify.Application.Bookings.CompleteBooking;
using Bookify.Application.Bookings.ConfirmBooking;
using Bookify.Application.Bookings.GetBooking;
using Bookify.Application.Bookings.ReserveBooking;
using Bookify.Domain.Abstractions;
using Bookify.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers.Bookings;

[ApiController]
[ApiVersion( ApiVersions.V1 )]
[Route( "api/v{version:apiVersion}/[controller]" )]
[HasPermission( Permissions.UsersRead )]
public class BookingsController( ISender sender ) : ControllerBase
{
    [HttpGet( "{id:guid}" )]
    public async Task<IActionResult> GetBooking( Guid id, CancellationToken cancellationToken )
    {
        GetBookingQuery query = new( id );

        Result<BookingResponse> result = await sender.Send( query, cancellationToken );

        return result.IsSuccess
            ? Ok( result.Value )
            : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> ReserveBooking(
        ReserveBookingRequest request,
        CancellationToken cancellationToken )
    {
        ReserveBookingCommand command = new(
            request.ApartmentId,
            request.UserId,
            request.Start,
            request.End );

        Result<Guid> result = await sender.Send( command, cancellationToken );
        if ( result.IsFailure )
        {
            return BadRequest( result.Error );
        }

        return CreatedAtAction( nameof( GetBooking ), new
        {
            id = result.Value
        }, result.Value );
    }

    [HttpPost( "{bookingId:guid}/confirm" )]
    public async Task<IActionResult> ConfirmBooking( Guid bookingId, CancellationToken cancellationToken )
    {
        ConfirmBookingCommand command = new( bookingId );

        Result result = await sender.Send( command, cancellationToken );
        if ( result.IsFailure )
        {
            return BadRequest( result.Error );
        }

        return Ok();
    }

    [HttpPost( "{bookingId:guid}/complete" )]
    public async Task<IActionResult> CompleteBooking( Guid bookingId, CancellationToken cancellationToken )
    {
        CompleteBookingCommand command = new( bookingId );

        Result result = await sender.Send( command, cancellationToken );
        if ( result.IsFailure )
        {
            return BadRequest( result.Error );
        }

        return Ok();
    }
}