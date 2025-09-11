using Asp.Versioning;
using Microsoft.OpenApi.Models;

namespace LinkUp.Presentation.API.Extensions;

public static class ServiceExtension
{
    public static void AddSwaggerExtension(this IServiceCollection services)
    {
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "LinkUp Api",
                Description = "",
                Contact = new OpenApiContact
                {
                    Name = "Team CodeGenius"
                }
            });
            option.EnableAnnotations();
        });
    }

    public static void AddVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified =
                true; //When no versions are sent, this assumes the default version which is V1
            options.ReportApiVersions = true;
        });
    }
}