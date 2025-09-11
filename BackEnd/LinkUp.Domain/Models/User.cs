using LinkUp.Domain.Common;
using LinkUp.Domain.Enum;

namespace LinkUp.Domain.Models;

public sealed class User : SoftDeletableEntity
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? ProfilePhoto { get; set; }

    public string? CoverPhoto { get; set; }

    public string? Biography { get; set; }

    public DateTime? Birthday { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public bool ConfirmedAccount { get; set; } = false;

    public UserStatus Status { get; set; } = UserStatus.Active;
    
    // Relationships
    
    public ICollection<RefreshToken>? RefreshToken { get; set; }
    
    public ICollection<Code> Codes { get; set; } = new List<Code>();
    
    public ICollection<UserInterest> UserInterests { get; set; } = new List<UserInterest>();

    public ICollection<PostLike> PostLikes { get; set; } = new List<PostLike>();

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}