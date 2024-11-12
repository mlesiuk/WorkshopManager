namespace workshopManager.Domain.Entities;

public class VehicleBodyType : BaseEntity
{
    public string Name { get; private set; } = string.Empty;

    private VehicleBodyType() { }

    public static VehicleBodyType Create(string name)
    {
        return new VehicleBodyType { Name = name };
    }
}
