using Asp.Versioning;
using Bookify.Application.Apartments.SearchApartments;
using Bookify.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers.Apartments;

[Authorize]
[ApiController]
[ApiVersion( ApiVersions.V1 )]
[Route( "api/v{version:apiVersion}/[controller]" )]
public class ApartmentsController : ControllerBase
{
    private readonly ISender _sender;

    public ApartmentsController( ISender sender )
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> SearchApartments(
        DateOnly startDate,
        DateOnly endDate )
    {
        SearchApartmentsQuery query = new( startDate, endDate );

        Result<IReadOnlyList<ApartmentResponse>> result = await _sender.Send( query );

        return Ok( result.Value );
    }
}