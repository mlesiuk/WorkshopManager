using workshopManager.Domain.Entities;

namespace workshopManager.Domain.Abstractions.Interfaces;

public interface IVehicleBrandRepository
{
    public Task AddAsync(VehicleBrand entity, CancellationToken cancellationToken = default);
    public Task<bool> AlreadyExistsAsync(VehicleBrand entity, CancellationToken cancellationToken = default);
    public Task<IEnumerable<VehicleBrand>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<VehicleBrand?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<VehicleBrand?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    public Task RemoveAsync(VehicleBrand entity, CancellationToken cancellationToken = default);
    public Task UpdateAsync(VehicleBrand entity, CancellationToken cancellationToken = default);
}
