using Bookify.Domain.Abstractions;

namespace Bookify.UnitTests.Infrastructure;

public class BaseTest
{
    protected static T AssertDomainEventWasPublished<T>( Entity entity )
    {
        T? domainEvent = entity.GetDomainEvents().OfType<T>().SingleOrDefault();

        return domainEvent ?? throw new Exception( $"{typeof( T ).Name} was not published" );
    }
}