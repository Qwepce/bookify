using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Bookify.Api.OpenApi;

public sealed class ConfigureSwaggerOptions( IApiVersionDescriptionProvider provider )
    : IConfigureNamedOptions<SwaggerGenOptions>
{
    public void Configure( SwaggerGenOptions options )
    {
        foreach ( ApiVersionDescription description in provider.ApiVersionDescriptions )
        {
            options.SwaggerDoc( description.GroupName, CreateVersionInfo( description ) );
        }
    }

    public void Configure( string? name, SwaggerGenOptions options )
    {
        Configure( options );
    }

    private static OpenApiInfo CreateVersionInfo( ApiVersionDescription apiVersionDescription )
    {
        OpenApiInfo openApiInfo = new()
        {
            Title = $"Bookify.Api v{apiVersionDescription.ApiVersion}",
            Version = apiVersionDescription.ApiVersion.ToString()
        };

        if ( apiVersionDescription.IsDeprecated )
        {
            openApiInfo.Description += " This API version is deprecated";
        }

        return openApiInfo;
    }
}