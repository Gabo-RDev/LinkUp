namespace LinkUp.Application.DTos.PostsCategories;

public sealed record CategoryDto(
    Guid CategoryId,
    string CategoryName,
    DateTime CreatedAt,
    DateTime UpdatedAt
);