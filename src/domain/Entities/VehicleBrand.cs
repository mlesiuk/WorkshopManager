namespace workshopManager.Domain.Entities;

public class VehicleBrand : BaseEntity
{
    public string Name { get; private set; } = string.Empty;

    private VehicleBrand() { }

    public static VehicleBrand Create(string name)
    {
        return new VehicleBrand { Name = name };
    }
}
