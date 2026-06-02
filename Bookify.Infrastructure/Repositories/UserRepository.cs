using Bookify.Domain.Users;

namespace Bookify.Infrastructure.Repositories;

internal sealed class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository( ApplicationDbContext context )
        : base( context )
    {
    }

    public override void Add( User entity )
    {
        foreach ( var role in entity.Roles )
        {
            DbContext.Attach( role );
        }

        DbContext.Add( entity );
    }
}
