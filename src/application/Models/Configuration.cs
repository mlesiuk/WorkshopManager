namespace workshopManager.Application.Models;

public record class Configuration
{
    public ConnectionStrings ConnectionStrings { get; set; } = new();
    public SecretClient SecretClient { get; set; } = new();
}

public class ConnectionStrings
{
    public string WorkshopManagerConnection { get; set; } = string.Empty;
}

public class SecretClient
{
    public string Name { get; set; } = string.Empty;
}