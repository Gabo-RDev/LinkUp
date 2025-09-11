using LinkUp.Domain.Common;

namespace LinkUp.Domain.Models;

public sealed class PostLike : SoftDeletableEntity
{
    public Guid PostId { get; set; }
    
    public Guid UserId { get; set; }

    public Post? Post { get; set; } 
    
    public User? User { get; set; }
}