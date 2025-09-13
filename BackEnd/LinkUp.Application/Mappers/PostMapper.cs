using LinkUp.Application.DTos.Posts;
using LinkUp.Domain.Models;

namespace LinkUp.Application.Mappers;

public static class PostMapper
{
    public static PostDto ToDto(Post post, Admin admin, PostCategory category)
    {
        return new PostDto
        (
            PostId: post.Id,
            Title: post.Title ?? string.Empty,
            Content: post.Content ?? string.Empty,
            LikesCount: post.LikesCount,
            AuthorPost: ToPostAdminDto(admin),
            Category: ToCategoryDto(category),
            CreatedAt: post.CreatedAt,
            UpdatedAt: post.UpdatedAt ?? post.CreatedAt
        );
    }

    public static PostDto ToDtoNavigation(Post post)
    {
        return new PostDto(
            PostId: post.Id,
            Title: post.Title ?? string.Empty,
            Content: post.Content ?? string.Empty,
            LikesCount: post.LikesCount,
            AuthorPost: ToPostAdminDto(post.Admin!), // property navigation
            Category: ToCategoryDto(post.Category!), // property navigation
            CreatedAt: post.CreatedAt,
            UpdatedAt: post.UpdatedAt ?? post.CreatedAt
        );
    }

    public static Post ToEntity(CreatePostDto dto)
    {
        return new Post
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Content = dto.Content,
            LikesCount = dto.LikesCount,
            AdminId = dto.AuthorPostId,
            CategoryId = dto.CategoryId
        };
    }

    #region Private Methods

    private static PostCategoryDto ToCategoryDto(PostCategory category)
    {
        return new PostCategoryDto(
            CategoryId: category.Id,
            CategoryName: category.Name!
        );
    }

    private static PostAdminDto ToPostAdminDto(Admin admin)
    {
        return new PostAdminDto
        (
            AdminId: admin.Id,
            AuthorName: admin.FirstName!,
            AuthorUsername: admin.UserName!,
            AuthorPhoto: admin.ProfilePhoto
        );
    }

    #endregion
}