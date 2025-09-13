using LinkUp.Application.Interfaces.Repository;
using LinkUp.Infrastructure.Persistence.Context;
using LinkUp.Infrastructure.Persistence.Repository;
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
    public static void AddInfrastructurePersistence(this IServiceCollection services,
        IConfiguration configuration)
    {
        #region AddRedis

        string connectionString = configuration.GetConnectionString("RedisConnection")!;
        services.AddStackExchangeRedisCache(options => { options.Configuration = connectionString; });

        #endregion

        #region AddDbContext

        services.AddDbContext<LinkUpDbContext>(db =>
            {
                db.UseNpgsql(configuration.GetConnectionString("LinkUpConnectionDb"),
                    b => b.MigrationsAssembly("LinkUp.Infrastructure.Persistence"));
            }
        );

        #endregion

        #region DI

        services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddTransient<IAdminRepository, AdminRepository>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<ICodeRepository, CodeRepository>();
        services.AddTransient<ICommentRepository, CommentRepository>();
        services.AddTransient<IInterestRepository, InterestRepository>();
        services.AddTransient<IPostCategoryRepository, PostCategoryRepository>();
        services.AddTransient<IPostRepository, PostRepository>();
        services.AddTransient<IPostInterestRepository, PostInterestRepository>();
        services.AddTransient<IPostLikeRepository, PostLikesRepository>();
        services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddTransient<IUserInterestRepository, UserInterestRepository>();

        #endregion
    }
}