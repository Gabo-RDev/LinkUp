using LinkUp.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LinkUp.Infrastructure.Persistence;

/// <summary>
/// Provides methods and extensions for configuring and managing
/// dependency injection within the application's infrastructure layer.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructurePersistence(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<LinkUpDbContext>(db =>
            {
                db.UseNpgsql(configuration.GetConnectionString("LinkUpConnectionDb"),
                    b => b.MigrationsAssembly("LinkUp.Infrastructure.Persistence"));
            }
        );

        return services;
    }
}