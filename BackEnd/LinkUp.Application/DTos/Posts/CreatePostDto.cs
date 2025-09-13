namespace LinkUp.Application.DTos.Posts;

public sealed record CreatePostDto(
    string Title,
    string Content,
    int LikesCount,
    Guid? AuthorPostId,
    Guid? CategoryId
);