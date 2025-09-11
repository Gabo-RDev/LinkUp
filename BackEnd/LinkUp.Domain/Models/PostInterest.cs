using LinkUp.Domain.Common;

namespace LinkUp.Domain.Models;

public sealed class PostInterest : SoftDeletableEntity
{
    public Guid PostId { get; set; }
    
    public Guid InterestId { get; set; }

    public Post? Post { get; set; }
    
    public Interest? Interest { get; set; } 
}