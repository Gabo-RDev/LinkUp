using LinkUp.Domain.Enum;

namespace LinkUp.Application.DTos.Posts;

public sealed record PostDto(
    Guid PostId,
    string Title,
    string Content,
    int LikesCount,
    PostAdminDto AuthorPost,
    PostCategoryDto Category,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public sealed record PostCategoryDto(
    Guid CategoryId,
    string CategoryName
);

public sealed record PostAdminDto(
    Guid AdminId,
    string AuthorName,
    string AuthorUsername,
    string? AuthorPhoto
);