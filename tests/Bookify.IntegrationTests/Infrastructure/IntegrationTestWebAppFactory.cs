using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Testcontainers.Keycloak;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;

namespace Bookify.IntegrationTests.Infrastructure;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage( "postgres:16" )
        .WithDatabase( "bookify" )
        .WithUsername( PostgreSqlBuilder.DefaultUsername )
        .WithPassword( PostgreSqlBuilder.DefaultPassword )
        .Build();

    private readonly RedisContainer _redis = new RedisBuilder()
        .WithImage( "redis:latest" )
        .Build();

    private readonly KeycloakContainer _keyCloak = new KeycloakBuilder()
        .WithResourceMapping(
            new FileInfo( ".files/bookify-realms-export.json" ),
            new FileInfo( "/opt/keycloak/data/import/realm.json" ) )
        .WithCommand( "--import-realm" )
        .Build();

    protected override void ConfigureWebHost( IWebHostBuilder builder )
    {
        builder.ConfigureTestServices( services =>
        {
            
        } );
    }
}