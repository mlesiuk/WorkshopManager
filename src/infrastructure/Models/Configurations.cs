namespace workshopManager.Infrastructure.Models;

public sealed class Configuration
{
    public RabbitMqConfiguration RabbitMq { get; set; } = new();
    public SecretClientConfiguration SecretClient { get; set; } = new();
}

public sealed class RabbitMqConfiguration
{
    public string Uri { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class SecretClientConfiguration
{
    public string Name { get; set; } = string.Empty;
}
