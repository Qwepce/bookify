using Bookify.Application.Bookings.CancelBooking;
using Bookify.Application.Bookings.CompleteBooking;
using Bookify.Application.Bookings.ConfirmBooking;
using Bookify.Application.Bookings.GetBooking;
using Bookify.Application.Bookings.RejectBooking;
using Bookify.Application.Bookings.ReserveBooking;
using Bookify.Domain.Abstractions;
using MediatR;

namespace Bookify.Api.Controllers.Bookings;

public static class BookingsEndpoints
{
    public static IEndpointRouteBuilder MapBookingEndpoints( this IEndpointRouteBuilder builder )
    {
        builder.MapGet( "bookings/{bookingId:guid}", GetBooking )
            .RequireAuthorization()
            .WithName( nameof( GetBooking ) );

        builder.MapPost( "bookings", ReserveBooking )
            .RequireAuthorization();

        builder.MapPost( "bookings/{bookingId:guid}/confirm", ConfirmBooking )
            .RequireAuthorization();

        builder.MapPost( "bookings/{bookingId:guid}/complete", CompleteBooking )
            .RequireAuthorization();

        builder.MapPost( "bookings/{bookingId:guid}/cancel", CancelBooking )
            .RequireAuthorization();

        builder.MapPost( "bookings/{bookingId:guid}/reject", RejectBooking )
            .RequireAuthorization();

        return builder;
    }

    private static async Task<IResult> GetBooking(
        Guid bookingId,
        ISender sender,
        CancellationToken cancellationToken )
    {
        GetBookingQuery query = new( bookingId );

        Result<BookingResponse> result = await sender.Send( query, cancellationToken );

        return result.IsSuccess
            ? Results.Ok( result.Value )
            : Results.NotFound();
    }

    private static async Task<IResult> ReserveBooking(
        ReserveBookingRequest request,
        ISender sender,
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
            return Results.BadRequest( result.Error );
        }

        return Results.CreatedAtRoute( nameof( GetBooking ), new
        {
            bookingId = result.Value
        }, result.Value );
    }

    private static async Task<IResult> ConfirmBooking(
        Guid bookingId,
        ISender sender,
        CancellationToken cancellationToken )
    {
        ConfirmBookingCommand command = new( bookingId );

        Result result = await sender.Send( command, cancellationToken );

        return ReturnResult( result );
    }

    private static async Task<IResult> CompleteBooking(
        Guid bookingId,
        ISender sender,
        CancellationToken cancellationToken )
    {
        CompleteBookingCommand command = new( bookingId );

        Result result = await sender.Send( command, cancellationToken );

        return ReturnResult( result );
    }

    private static async Task<IResult> CancelBooking(
        Guid bookingId,
        ISender sender,
        CancellationToken cancellationToken )
    {
        CancelBookingCommand command = new( bookingId );

        Result result = await sender.Send( command, cancellationToken );

        return ReturnResult( result );
    }

    private static async Task<IResult> RejectBooking(
        Guid bookingId,
        ISender sender,
        CancellationToken cancellationToken )
    {
        RejectBookingCommand command = new( bookingId );

        Result result = await sender.Send( command, cancellationToken );

        return ReturnResult( result );
    }

    private static IResult ReturnResult( Result result )
    {
        return result.IsFailure
            ? Results.BadRequest( result.Error )
            : Results.Ok();
    }
}