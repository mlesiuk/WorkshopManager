using workshopManager.Domain.Entities;

namespace workshopManager.Application.Abstractions.Interfaces;

public interface IVehicleRepository
{
    public Task AddAsync(Vehicle entity, CancellationToken cancellationToken = default);
    public Task<bool> AlreadyExistsAsync(Vehicle entity, CancellationToken cancellationToken = default);
    public Task<IEnumerable<Vehicle>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task RemoveAsync(Vehicle entity, CancellationToken cancellationToken = default);
    public Task UpdateAsync(Vehicle entity, CancellationToken cancellationToken = default);
}
