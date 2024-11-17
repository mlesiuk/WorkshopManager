using workshopManager.Domain.Entities;

namespace workshopManager.Application.Abstractions.Interfaces;

public interface IVehicleAdditionalEquipmentRepository
{
    public Task AddAsync(VehicleAdditionalEquipment entity, CancellationToken cancellationToken = default);
    public Task<bool> AlreadyExistsAsync(VehicleAdditionalEquipment entity, CancellationToken cancellationToken = default);
    public Task<IEnumerable<VehicleAdditionalEquipment>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<VehicleAdditionalEquipment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<VehicleAdditionalEquipment?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    public Task RemoveAsync(VehicleAdditionalEquipment entity, CancellationToken cancellationToken = default);
    public Task UpdateAsync(VehicleAdditionalEquipment entity, CancellationToken cancellationToken = default);
}
