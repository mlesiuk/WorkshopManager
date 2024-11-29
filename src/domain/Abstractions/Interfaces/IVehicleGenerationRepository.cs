using workshopManager.Domain.Entities;

namespace workshopManager.Domain.Abstractions.Interfaces;

public interface IVehicleGenerationRepository
{
    public Task AddAsync(VehicleGeneration entity, CancellationToken cancellationToken = default);
    public Task<bool> AlreadyExistsAsync(VehicleGeneration entity, CancellationToken cancellationToken = default);
    public Task<IEnumerable<VehicleGeneration>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<VehicleGeneration?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<VehicleGeneration?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    public Task RemoveAsync(VehicleGeneration entity, CancellationToken cancellationToken = default);
    public Task UpdateAsync(VehicleGeneration entity, CancellationToken cancellationToken = default);
}
