namespace LinkUp.Domain.Common;

public abstract class SoftDeletableEntity : AuditableEntity
{
    public bool Deleted { get; set; } = false;
    
    public DateTime? DeletedAt { get; set; }
}