using workshopManager.Domain.Entities;

namespace workshopManager.Application.Abstractions.Interfaces;

public interface IVehicleEngineRepository
{
    public Task AddAsync(VehicleEngine entity, CancellationToken cancellationToken = default);
    public Task<bool> AlreadyExistsAsync(VehicleEngine entity, CancellationToken cancellationToken = default);
    public Task<IEnumerable<VehicleEngine>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<VehicleEngine?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<VehicleEngine?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    public Task RemoveAsync(VehicleEngine entity, CancellationToken cancellationToken = default);
    public Task UpdateAsync(VehicleEngine entity, CancellationToken cancellationToken = default);
}
