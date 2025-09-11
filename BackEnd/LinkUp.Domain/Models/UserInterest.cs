using LinkUp.Domain.Common;

namespace LinkUp.Domain.Models;

public sealed class UserInterest : SoftDeletableEntity
{
    public Guid UserId { get; set; }

    public Guid InterestId { get; set; }

    public User? User { get; set; }

    public Interest? Interest { get; set; }
}