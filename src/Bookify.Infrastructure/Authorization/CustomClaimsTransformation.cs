using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Bookify.Domain.Users;
using Bookify.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Bookify.Infrastructure.Authorization;

internal sealed class CustomClaimsTransformation( IServiceProvider serviceProvider ) : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync( ClaimsPrincipal principal )
    {
        if ( principal.HasClaim( claim => claim.Type == ClaimTypes.Role ) &&
             principal.HasClaim( claim => claim.Type == JwtRegisteredClaimNames.Sub ) )
        {
            return principal;
        }

        IServiceScope scope = serviceProvider.CreateScope();

        AuthorizationService authorizationService = scope.ServiceProvider.GetRequiredService<AuthorizationService>();

        string identityId = principal.GetIdentityId();

        UserRolesResponse userRoles = await authorizationService.GetRolesForUserAsync( identityId );

        ClaimsIdentity claimsIdentity = new();

        claimsIdentity.AddClaim( new Claim( JwtRegisteredClaimNames.Sub, userRoles.Id.ToString() ) );

        foreach ( Role role in userRoles.Roles )
        {
            claimsIdentity.AddClaim( new Claim( ClaimTypes.Role, role.Name ) );
        }

        principal.AddIdentity( claimsIdentity );

        return principal;
    }
}