using LinkUp.Application.DTos.Posts;
using LinkUp.Application.DTos.PostsCategories;
using LinkUp.Domain.Models;

namespace LinkUp.Application.Mappers;

public static class PostCategoryMapper
{
    public static CategoryDto ToDto(PostCategory postCategory)
    {
        return new CategoryDto
        (
            CategoryId: postCategory.Id,
            CategoryName: postCategory.Name!,
            CreatedAt: postCategory.CreatedAt,
            UpdatedAt: postCategory.UpdatedAt ?? postCategory.CreatedAt
        );
    }

    public static PostCategory ToEntity(CreatePostCategoryDto dto)
    {
        return new PostCategory
        {
            Id = Guid.NewGuid(),
            Name = dto.CategoryName
        };
    }
}