using Serilog;

namespace LinkUp.Presentation.API.Extensions;

/// <summary>
/// Provides extension methods for setting up and configuring the application.
/// </summary>
public static class Extension
{
    public static void AddSerilogExtension(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration.ReadFrom.Configuration(context.Configuration);
        });
    }
}