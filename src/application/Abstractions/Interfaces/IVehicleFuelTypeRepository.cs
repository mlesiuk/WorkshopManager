using workshopManager.Domain.Entities;

namespace workshopManager.Application.Abstractions.Interfaces;

public interface IVehicleFuelTypeRepository
{
    public Task AddAsync(VehicleFuelType entity, CancellationToken cancellationToken = default);
    public Task<bool> AlreadyExistsAsync(VehicleFuelType entity, CancellationToken cancellationToken = default);
    public Task<IEnumerable<VehicleFuelType>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<VehicleFuelType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<VehicleFuelType?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    public Task RemoveAsync(VehicleFuelType entity, CancellationToken cancellationToken = default);
    public Task UpdateAsync(VehicleFuelType entity, CancellationToken cancellationToken = default);
}
