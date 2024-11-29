using workshopManager.Domain.Entities;

namespace workshopManager.Domain.Abstractions.Interfaces;

public interface IWorkerRepository
{
    public Task<Worker?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
