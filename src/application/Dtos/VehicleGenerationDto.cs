namespace workshopManager.Application.Dtos;

public record class VehicleGenerationDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
}
