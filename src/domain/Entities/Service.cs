namespace workshopManager.Domain.Entities;

public sealed class Service : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    private Service(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public static Service Create(string name, string description)
    {
        return new Service(name, description);
    }

    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
