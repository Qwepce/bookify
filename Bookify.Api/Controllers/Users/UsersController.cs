using Asp.Versioning;
using Bookify.Application.Users.GetLoggedInUser;
using Bookify.Application.Users.LoginUser;
using Bookify.Application.Users.RegisterUser;
using Bookify.Domain.Abstractions;
using Bookify.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers.Users;

[ApiController]
[ApiVersion( ApiVersions.V1 )]
[Route( "api/v{version:apiVersion}/[controller]" )]
public class UsersController( ISender sender ) : ControllerBase
{
    [HttpGet( "me" )]
    [HasPermission( Permissions.UsersRead )]
    public async Task<IActionResult> GetLoggedInUserV1( CancellationToken cancellationToken )
    {
        GetLoggedInUserQuery query = new();

        Result<UserResponse> result = await sender.Send( query, cancellationToken );

        return Ok( result.Value );
    }

    [AllowAnonymous]
    [HttpPost( "register" )]
    public async Task<IActionResult> Register(
        RegisterUserRequest request,
        CancellationToken cancellationToken )
    {
        RegisterUserCommand command = new(
            request.Email,
            request.FirstName,
            request.LastName,
            request.Password );

        Result<Guid> result = await sender.Send( command, cancellationToken );
        if ( result.IsFailure )
        {
            return BadRequest( result.Error );
        }

        return Ok( result.Value );
    }

    [AllowAnonymous]
    [HttpPost( "login" )]
    public async Task<IActionResult> Login(
        LogInUserRequest request,
        CancellationToken cancellationToken )
    {
        LogInUserCommand command = new(
            request.Email,
            request.Password );

        Result<AccessTokenResponse> result = await sender.Send( command, cancellationToken );
        if ( result.IsFailure )
        {
            return BadRequest( result.Error );
        }

        return Ok( result.Value );
    }
}