using Bookify.Domain.Users;
using Bookify.Domain.Users.Events;
using Bookify.UnitTests.Infrastructure;
using FluentAssertions;

namespace Bookify.UnitTests.Users;

public class UserTests : BaseTest
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
        UserCreatedDomainEvent domainEvent = AssertDomainEventWasPublished<UserCreatedDomainEvent>( user );
        domainEvent.UserId.Should().Be( user.Id );
    }

    [Fact]
    public void Create_Should_AddRegisteredRoleToUser()
    {
        // Act
        User user = User.Create( UserData.FirstName, UserData.LastName, UserData.Email );

        // Assert
        user.Roles.Should().NotBeEmpty();
        user.Roles.Should().Contain( Role.Registered );
    }
}