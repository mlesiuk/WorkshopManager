using workshopManager.Domain.Entities;

namespace workshopManager.Application.Abstractions.Interfaces;

public interface IWorkerRepository
{
    public Task<Worker?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
