using System.Text.Json;
using workshopManager.Presentation.Models;

namespace workshopManager.Presentation.Services;

public interface IApiService
{
    public Task<VehicleBrand?> GetVehicleBrandByIdAsync(string id, CancellationToken cancellationToken = default);
    public Task<IEnumerable<VehicleBrand>> GetVehicleBrandsAsync(CancellationToken cancellationToken = default);
}

public class ApiService(IHttpClientFactory httpClientFactory) : IApiService
{
    public async Task<VehicleBrand?> GetVehicleBrandByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient("workshopManagerApi");
        var url = $"{client.BaseAddress}vehicleBrand/{id}";
        var response = await client.GetAsync(url, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return default;
        }

        var content = await response.Content.ReadAsByteArrayAsync(cancellationToken);
        if (content?.Length == 0)
        {
            return default;
        }

        var result = JsonSerializer.Deserialize<VehicleBrand>(content);
        if (result is null)
        {
            return default;
        }

        return result;
    }

    public async Task<IEnumerable<VehicleBrand>> GetVehicleBrandsAsync(CancellationToken cancellationToken = default)
    {
        var client = httpClientFactory.CreateClient("workshopManagerApi");
        var url = $"{client.BaseAddress}vehicleBrand";
        var response = await client.GetAsync(url, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return [];
        }

        var content = await response.Content.ReadAsByteArrayAsync(cancellationToken);
        if (content?.Length == 0)
        {
            return [];
        }

        var result = JsonSerializer.Deserialize<IEnumerable<VehicleBrand>>(content);
        if (result is null)
        {
            return [];
        }

        return result;
    }
}
