namespace workshopManager.Domain.Entities;

public class VehicleGearbox : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    private VehicleGearbox() { }

    public static VehicleGearbox Create(string name)
    {
        return new VehicleGearbox { Name = name };
    }

    public void UpdateName(string name)
    {
        Name = name;
    }
}
