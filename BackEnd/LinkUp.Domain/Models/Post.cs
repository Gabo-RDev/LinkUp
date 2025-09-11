using LinkUp.Domain.Common;

namespace LinkUp.Domain.Models;

public sealed class Post : SoftDeletableEntity
{
    public string? Title { get; set; }

    public string? Content { get; set; }

    public Guid? AdminId { get; set; }

    public Guid? CategoryId { get; set; }

    public int LikesCount { get; set; } = 0;

    public Admin? Admin { get; set; }
    
    public PostCategory? Category { get; set; }
    
    public ICollection<PostInterest> PostInterests { get; set; } = new List<PostInterest>();
    
    public ICollection<PostLike> PostLikes { get; set; } = new List<PostLike>();
    
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}