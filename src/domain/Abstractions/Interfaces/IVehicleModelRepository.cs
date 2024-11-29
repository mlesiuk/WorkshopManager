using workshopManager.Domain.Entities;

namespace workshopManager.Domain.Abstractions.Interfaces;

public interface IVehicleModelRepository
{
    public Task AddAsync(VehicleModel entity, CancellationToken cancellationToken = default);
    public Task<bool> AlreadyExistsAsync(VehicleModel entity, CancellationToken cancellationToken = default);
    public Task<IEnumerable<VehicleModel>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<VehicleModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<VehicleModel?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    public Task RemoveAsync(VehicleModel entity, CancellationToken cancellationToken = default);
    public Task UpdateAsync(VehicleModel entity, CancellationToken cancellationToken = default);
}
