using System.Reflection;
using Bookify.ArchitectureTests.Infrastructure;
using Bookify.Domain.Abstractions;
using FluentAssertions;
using NetArchTest.Rules;

namespace Bookify.ArchitectureTests.Domain;

public class DomainTests : BaseTest
{
    [Fact]
    public void DomainEvents_Should_BeSealed()
    {
        IEnumerable<Type>? types = Types.InAssembly( DomainAssembly )
            .That()
            .ImplementInterface( typeof( IDomainEvent ) )
            .GetTypes();

        List<Type> nonSealedEvents = types.Where( type => !type.IsSealed ).ToList();

        nonSealedEvents.Should().BeEmpty(
            "All domain events should be sealed.\n" +
            $"Non sealed domain events: {string.Join( ", ", nonSealedEvents.Select( type => type.Name ) )}" );
    }

    [Fact]
    public void DomainEvent_ShouldHave_DomainEventPostfix()
    {
        TestResult? result = Types.InAssembly( DomainAssembly )
            .That()
            .ImplementInterface( typeof( IDomainEvent ) )
            .Should()
            .HaveNameEndingWith( "DomainEvent" )
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Entities_ShouldHave_PrivateParameterlessConstructor()
    {
        IEnumerable<Type>? types = Types.InAssembly( DomainAssembly )
            .That()
            .Inherit( typeof( Entity ) )
            .GetTypes();

        List<Type> failingTypes = [];
        foreach ( Type type in types )
        {
            ConstructorInfo[] constructors = type.GetConstructors( BindingFlags.NonPublic | BindingFlags.Instance );

            if ( constructors.All( constructor => constructor.GetParameters().Length != 0 ) )
            {
                failingTypes.Add( type );
            }
        }

        failingTypes.Should().BeEmpty();
    }
}