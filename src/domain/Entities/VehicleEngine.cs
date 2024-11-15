namespace workshopManager.Domain.Entities;

public class VehicleEngine : BaseEntity
{
    public string Name { get; private set; } = string.Empty;

    private VehicleEngine() { }

    public static VehicleEngine Create(string name)
    {
        return new VehicleEngine { Name = name };
    }

    public void UpdateName(string name)
    {
        Name = name;
    }
}
