using LinkUp.Application.Interfaces.Repository;
using LinkUp.Application.Pagination;
using LinkUp.Domain.Models;
using LinkUp.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Infrastructure.Persistence.Repository;

public class PostLikesRepository(LinkUpDbContext context): GenericRepository<PostLike>(context), IPostLikeRepository
{

    public async Task<int> GetLikesCountAsync(Guid postId, CancellationToken cancellationToken) =>
        await context.Set<PostLike>()
            .Where(c => c.PostId == postId)
            .CountAsync(cancellationToken);

    public async Task<bool> HasUserLikedPostAsync(Guid postId, Guid userId, CancellationToken cancellationToken) =>
        await context.Set<PostLike>()
            .Where(c => c.PostId == postId && c.UserId == userId)
            .AnyAsync(cancellationToken);
}