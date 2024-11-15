namespace workshopManager.Domain.Entities;

public class VehicleFuelType : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    private VehicleFuelType() { }

    public static VehicleFuelType Create(string name)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

        return new()
        {
            Name = name
        };
    }

    public void UpdateName(string name)
    {
        Name = name;
    }
}
