namespace workshopManager.Application.Abstractions.Interfaces;

public interface ISecretsService
{
    public Task<string> GetSecretAsync(string name, CancellationToken cancellationToken = default);
    public Task SetSecretAsync(string name, string value, CancellationToken cancellationToken = default);
}
