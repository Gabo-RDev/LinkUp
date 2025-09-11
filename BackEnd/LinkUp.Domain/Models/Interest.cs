using LinkUp.Domain.Common;

namespace LinkUp.Domain.Models;

public sealed class Interest : SoftDeletableEntity
{
    public string? Name { get; set; } 

    public ICollection<UserInterest> UserInterests { get; set; } = new List<UserInterest>();

    public ICollection<PostInterest> PostInterests { get; set; } = new List<PostInterest>();
}