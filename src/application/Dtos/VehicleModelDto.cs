namespace workshopManager.Application.Dtos;

public record class VehicleModelDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
}

