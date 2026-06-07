using Bookify.Domain.Users;
using Bookify.Domain.Users.Events;
using FluentAssertions;

namespace Bookify.Domain.UnitTests.Users;

public class UserTests
{
    [Fact]
    public void Create_Should_SetPropertyValues()
    {
        // Act
        User user = User.Create( UserData.FirstName, UserData.LastName, UserData.Email );

        // Assert
        user.Should().NotBeNull();
        user.FirstName.Should().Be( UserData.FirstName );
        user.LastName.Should().Be( UserData.LastName );
        user.Email.Should().Be( UserData.Email );
    }

    [Fact]
    public void Create_Should_RaiseDomainEvent()
    {
        // Act
        User user = User.Create( UserData.FirstName, UserData.LastName, UserData.Email );

        // Assert
        UserCreatedDomainEvent? domainEvent = user.GetDomainEvents().OfType<UserCreatedDomainEvent>().SingleOrDefault();
        domainEvent?.UserId.Should().Be( user.Id );
    }
}

internal static class UserData
{
    public static readonly FirstName FirstName = new( "John" );
    public static readonly LastName LastName = new( "Doe" );
    public static readonly Email Email = new( "johndoe@example.test" );
}