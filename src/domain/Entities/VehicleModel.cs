namespace workshopManager.Domain.Entities;

public class VehicleModel : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    private VehicleModel() { }

    public static VehicleModel Create(string name)
    {
        return new VehicleModel { Name = name };
    }

    public void UpdateName(string name)
    {
        Name = name;
    }
}
