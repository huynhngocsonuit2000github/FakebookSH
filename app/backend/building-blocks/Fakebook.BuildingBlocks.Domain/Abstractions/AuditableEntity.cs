namespace Fakebook.BuildingBlocks.Domain.Abstractions;

public abstract class AuditableEntity : Entity
{
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAtUtc { get; set; } = DateTime.UtcNow;
}