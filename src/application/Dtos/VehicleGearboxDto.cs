namespace workshopManager.Application.Dtos;

public record class VehicleGearboxDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
