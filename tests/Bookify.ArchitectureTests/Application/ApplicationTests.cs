using Bookify.Application.Abstractions.Messaging;
using Bookify.ArchitectureTests.Infrastructure;
using FluentAssertions;
using FluentValidation;
using NetArchTest.Rules;

namespace Bookify.ArchitectureTests.Application;

public class ApplicationTests : BaseTest
{
    [Fact]
    public void CommandHandler_ShouldHave_NameEndingWith_CommandHandler()
    {
        TestResult? result = Types.InAssembly( ApplicationAssembly )
            .That()
            .ImplementInterface( typeof( ICommandHandler<> ) )
            .Or()
            .ImplementInterface( typeof( ICommandHandler<,> ) )
            .Should()
            .HaveNameEndingWith( "CommandHandler" )
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void CommandHandler_ShouldBe_InternalAndSealed()
    {
        IEnumerable<Type>? types = Types.InAssembly( ApplicationAssembly )
            .That()
            .ImplementInterface( typeof( ICommandHandler<> ) )
            .Or()
            .ImplementInterface( typeof( ICommandHandler<,> ) )
            .GetTypes();

        List<Type> failingTypes = types.Where( type => !type.IsNotPublic || !type.IsSealed ).ToList();

        failingTypes.Should().BeEmpty(
            "There are few command handlers which is public or not sealed:\n" +
            $"{string.Join( ";\n", failingTypes.Select( t => t.Name ) )}" + Environment.NewLine );
    }


    [Fact]
    public void QueryHandler_ShouldHave_NameEndingWith_QueryHandler()
    {
        IEnumerable<Type>? types = Types.InAssembly( ApplicationAssembly )
            .That()
            .ImplementInterface( typeof( IQueryHandler<,> ) )
            .GetTypes();

        List<Type> failingTypes = types.Where( type => !type.Name.EndsWith( "QueryHandler" ) ).ToList();

        failingTypes.Should().BeEmpty(
            "There are few query handlers which name doesn't end with 'QueryHandler':\n" +
            $"{string.Join( ";\n", failingTypes.Select( t => t.Name ) )}" + Environment.NewLine );
    }

    [Fact]
    public void QueryHandler_ShouldBe_InternalAndSealed()
    {
        IEnumerable<Type>? types = Types.InAssembly( ApplicationAssembly )
            .That()
            .ImplementInterface( typeof( IQueryHandler<,> ) )
            .GetTypes();

        List<Type> failingTypes = types.Where( type => !type.IsNotPublic || !type.IsSealed ).ToList();

        failingTypes.Should().BeEmpty(
            "There are few query handlers which is public or not sealed:\n" +
            $"{string.Join( ";\n", failingTypes.Select( t => t.Name ) )}" + Environment.NewLine );
    }

    [Fact]
    public void Validator_Name_ShouldHave_NameEndingWith_Validator()
    {
        TestResult? result = Types.InAssembly( ApplicationAssembly )
            .That()
            .Inherit( typeof( AbstractValidator<> ) )
            .Should()
            .HaveNameEndingWith( "Validator" )
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Validator_ShouldBe_SealedNotBePublic()
    {
        TestResult? result = Types.InAssembly( ApplicationAssembly )
            .That()
            .Inherit( typeof( AbstractValidator<> ) )
            .Should()
            .NotBePublic()
            .And()
            .BeSealed()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}