using Azure.Security.KeyVault.Secrets;
using workshopManager.Application.Abstractions.Interfaces;

namespace workshopManager.Infrastructure.Services;

public class SecretsService(SecretClient secretClient) : ISecretsService
{
    public async Task<string> GetSecretAsync(string name, CancellationToken cancellationToken = default)
    {
        var response = await secretClient.GetSecretAsync(name, null, cancellationToken);
        if (response == null)
        {
            return string.Empty;
        }

        return response.Value.Value;
    }

    public async Task SetSecretAsync(string name, string value, CancellationToken cancellationToken = default)
    {
        var _ = await secretClient.SetSecretAsync(name, value, cancellationToken);
    }
}
