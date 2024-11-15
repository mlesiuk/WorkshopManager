using workshopManager.Domain.Entities;

namespace workshopManager.Application.Abstractions.Interfaces;

public interface IVehicleBodyTypeRepository
{
    public Task AddAsync(VehicleBodyType entity, CancellationToken cancellationToken = default);
    public Task<bool> AlreadyExistsAsync(VehicleBodyType entity, CancellationToken cancellationToken = default);
    public Task<IEnumerable<VehicleBodyType>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<VehicleBodyType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<VehicleBodyType?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    public Task RemoveAsync(VehicleBodyType entity, CancellationToken cancellationToken = default);
    public Task UpdateAsync(VehicleBodyType entity, CancellationToken cancellationToken = default);
}
