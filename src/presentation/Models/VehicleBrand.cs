using System.Text.Json.Serialization;

namespace workshopManager.Presentation.Models;

public sealed class VehicleBrand
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}
