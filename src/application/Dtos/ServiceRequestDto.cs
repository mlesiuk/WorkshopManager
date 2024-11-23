using workshopManager.Domain.Enums;

namespace workshopManager.Application.Dtos;

public record class ServiceRequestDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public CustomerDto Customer { get; set; } = new();
    public VehicleDto Vehicle { get; set; } = new();
    public ServiceStatus Status { get; set; }
}
