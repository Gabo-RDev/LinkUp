using LinkUp.Domain.Common;
using LinkUp.Domain.Enum;

namespace LinkUp.Domain.Models;

public sealed class Admin : SoftDeletableEntity
{
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }
    
    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }
    
    public string? ProfilePhoto { get; set; }
    
    public DateTime? LastLoginAt { get; set; }
    
    public AdminStatus Status { get; set; } = AdminStatus.Active;
    
    public bool ConfirmedAccount { get; set; } = false;

    public ICollection<Post> Posts { get; set; } = new List<Post>();
}