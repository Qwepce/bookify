using Bookify.Api.Extensions;
using Bookify.Application;
using Bookify.Infrastructure;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder( args );

builder.Host.UseSerilog( ( context, configuration ) =>
{
    configuration.ReadFrom.Configuration( context.Configuration );
} );

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure( builder.Configuration );

WebApplication app = builder.Build();

if ( app.Environment.IsDevelopment() )
{
    app.MapOpenApi();
    app.UseSwagger();

    app.UseSwaggerUI( options =>
    {
        options.SwaggerEndpoint( "/openapi/v1.json", "Bookify API V1" );
    } );

    app.ApplyMigrations();
    //app.ApplySeedData();
}

app.UseHttpsRedirection();

app.UseRequestContextLogging();

app.UseSerilogRequestLogging();

app.UseCustomExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();