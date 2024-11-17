using workshopManager.Domain.Entities;

namespace workshopManager.Application.Abstractions.Interfaces;

public interface IVehicleGearboxRepository
{
    public Task AddAsync(VehicleGearbox entity, CancellationToken cancellationToken = default);
    public Task<bool> AlreadyExistsAsync(VehicleGearbox entity, CancellationToken cancellationToken = default);
    public Task<IEnumerable<VehicleGearbox>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<VehicleGearbox?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<VehicleGearbox?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    public Task RemoveAsync(VehicleGearbox entity, CancellationToken cancellationToken = default);
    public Task UpdateAsync(VehicleGearbox entity, CancellationToken cancellationToken = default);
}
