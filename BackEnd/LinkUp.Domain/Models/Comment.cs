using LinkUp.Domain.Common;

namespace LinkUp.Domain.Models;

public sealed class Comment : SoftDeletableEntity
{
    public string? Description { get; set; }

    public Guid PostId { get; set; }

    public Guid UserId { get; set; }

    public bool IsPinned { get; set; } = false;

    public Guid? ParentCommentId { get; set; }

    public bool Edited { get; set; } = false;

    public Post? Post { get; set; }

    public User? User { get; set; }
}