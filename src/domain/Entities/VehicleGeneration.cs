namespace workshopManager.Domain.Entities;

public class VehicleGeneration : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    private VehicleGeneration() { }

    public static VehicleGeneration Create(string name)
    {
        return new VehicleGeneration { Name = name };
    }

    public void UpdateName(string name)
    {
        Name = name;
    }
}
