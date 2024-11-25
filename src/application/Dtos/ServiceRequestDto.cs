using workshopManager.Domain.Enums;

namespace workshopManager.Application.Dtos;

public record class ServiceRequestDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid CustomerId { get; set; } = Guid.Empty;
    public Guid VehicleId { get; set; } = Guid.Empty;
    public DateTime ServiceDate { get; set; }
}
