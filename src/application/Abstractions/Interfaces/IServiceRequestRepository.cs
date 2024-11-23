using workshopManager.Domain.Entities;
using workshopManager.Domain.Enums;

namespace workshopManager.Application.Abstractions.Interfaces;

public interface IServiceRequestRepository
{
    public Task AddAsync(ServiceRequest entity, CancellationToken cancellationToken = default);
    public Task<bool> AlreadyExistsAsync(ServiceRequest entity, CancellationToken cancellationToken = default);
    public Task<IEnumerable<ServiceRequest>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<ServiceRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<ServiceRequest?> GetByStatusAsync(ServiceStatus status, CancellationToken cancellationToken = default);
    public Task RemoveAsync(ServiceRequest entity, CancellationToken cancellationToken = default);
    public Task UpdateAsync(ServiceRequest entity, CancellationToken cancellationToken = default);
}
