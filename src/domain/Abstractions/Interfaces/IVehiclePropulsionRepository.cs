using workshopManager.Domain.Entities;

namespace workshopManager.Domain.Abstractions.Interfaces;

public interface IVehiclePropulsionRepository
{
    public Task AddAsync(VehiclePropulsion entity, CancellationToken cancellationToken = default);
    public Task<bool> AlreadyExistsAsync(VehiclePropulsion entity, CancellationToken cancellationToken = default);
    public Task<IEnumerable<VehiclePropulsion>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<VehiclePropulsion?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<VehiclePropulsion?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    public Task RemoveAsync(VehiclePropulsion entity, CancellationToken cancellationToken = default);
    public Task UpdateAsync(VehiclePropulsion entity, CancellationToken cancellationToken = default);
}
