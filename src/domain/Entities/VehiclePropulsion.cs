namespace workshopManager.Domain.Entities;

public class VehiclePropulsion : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    private VehiclePropulsion() { }

    public static VehiclePropulsion Create(string name)
    {
        return new VehiclePropulsion { Name = name };
    }

    public void UpdateName(string name)
    {
        Name = name;
    }
}
