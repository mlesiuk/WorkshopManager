using workshopManager.Domain.Entities;

namespace workshopManager.Application.Abstractions.Interfaces;

public interface IVehicleBrandRepository
{
    public Task AddAsync(VehicleBrand entity, CancellationToken cancellationToken = default);
    public Task<bool> AlreadyExistsAsync(VehicleBrand vehicleBrand, CancellationToken cancellationToken = default);
    public Task<VehicleBrand?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<VehicleBrand?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}
