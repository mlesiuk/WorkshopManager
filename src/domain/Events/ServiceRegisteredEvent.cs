namespace workshopManager.Domain.Events;

public sealed class ServiceRegisteredEvent
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid CustomerId { get; set; } = Guid.Empty;
    public Guid VehicleId { get; set; } = Guid.Empty;
}
