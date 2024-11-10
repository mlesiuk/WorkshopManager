using workshopManager.Domain.Common;

namespace workshopManager.Domain.Entities;

public class BaseEntity : AuditableEntity
{
    public Guid Id { get; set; }
    public long ClusterId { get; set; }
}
