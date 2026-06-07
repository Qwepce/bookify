using Bookify.Application.Cache;
using Bookify.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Authorization;

internal sealed class AuthorizationService( ApplicationDbContext dbContext, ICacheService cacheService )
{
    public async Task<UserRolesResponse> GetRolesForUserAsync( string identityId )
    {
        string cacheKey = $"auth:roles-{identityId}";
        UserRolesResponse? cachedRoles = await cacheService.GetAsync<UserRolesResponse>( cacheKey );
        if ( cachedRoles is not null )
        {
            return cachedRoles;
        }

        UserRolesResponse roles = await dbContext.Set<User>()
            .Where( user => user.IdentityId == identityId )
            .Select( user => new UserRolesResponse
            {
                Id = user.Id,
                Roles = user.Roles.ToList()
            } )
            .FirstAsync();

        await cacheService.SetAsync( cacheKey, roles );

        return roles;
    }

    internal async Task<HashSet<string>> GetPermissionsForUserAsync( string identityId )
    {
        string cacheKey = $"auth:permissions-{identityId}";
        HashSet<string>? cachedPermissions = await cacheService.GetAsync<HashSet<string>>( cacheKey );
        if ( cachedPermissions is not null )
        {
            return cachedPermissions;
        }

        ICollection<Permission> permissions = await dbContext.Set<User>()
            .Where( user => user.IdentityId == identityId )
            .SelectMany( user => user.Roles.Select( role => role.Permissions ) )
            .FirstAsync();

        HashSet<string> permissionsSet = permissions.Select( permission => permission.Name ).ToHashSet();

        await cacheService.SetAsync( cacheKey, permissionsSet );

        return permissionsSet;
    }
}