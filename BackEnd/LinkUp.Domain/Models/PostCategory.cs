using LinkUp.Domain.Common;

namespace LinkUp.Domain.Models;

public class PostCategory : SoftDeletableEntity
{
    public string? Name { get; set; }
    public ICollection<Post> Posts { get; set; } = new List<Post>();
}