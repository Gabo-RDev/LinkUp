using LinkUp.Domain.Common;
using LinkUp.Domain.Enum;

namespace LinkUp.Domain.Models;

public sealed class Code : AuditableEntity
{
    public Guid UserId { get; set; }

    public string? Value { get; set; }

    public DateTime Expiration { get; set; }

    public CodeType Type { get; set; }

    public bool Revoked { get; set; } = false;
    public bool Used { get; set; } = false;

    public User? User { get; set; }
}