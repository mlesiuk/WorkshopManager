using Microsoft.EntityFrameworkCore;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Domain.Entities;
using workshopManager.Infrastructure.Persistence;

namespace workshopManager.Infrastructure.Repositories;

public sealed class VehicleAdditionalEquipmentRepository(ApplicationDbContext context) : IVehicleAdditionalEquipmentRepository
{
    private readonly DbSet<VehicleAdditionalEquipment> _vehicleAdditionalEquipment = context.Set<VehicleAdditionalEquipment>();

    public async Task AddAsync(VehicleAdditionalEquipment entity, CancellationToken cancellationToken = default)
    {
        await _vehicleAdditionalEquipment.AddAsync(entity, cancellationToken);
    }

    public async Task<bool> AlreadyExistsAsync(VehicleAdditionalEquipment entity, CancellationToken cancellationToken = default)
    {
        if (await GetByIdAsync(entity.Id, cancellationToken) is not null)
        {
            return true;
        }

        if (await GetByNameAsync(entity.Name, cancellationToken) is not null)
        {
            return true;
        }

        return false;
    }

    public async Task<IEnumerable<VehicleAdditionalEquipment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _vehicleAdditionalEquipment
            .Where(vae => vae.Deleted == null)
            .ToListAsync(cancellationToken) ?? [];
    }

    public async Task<VehicleAdditionalEquipment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _vehicleAdditionalEquipment
            .SingleOrDefaultAsync(vae => vae.Id == id && vae.Deleted == null, cancellationToken);
    }

    public async Task<VehicleAdditionalEquipment?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _vehicleAdditionalEquipment
            .FirstOrDefaultAsync(vae => EF.Functions.Like(vae.Name, name) && vae.Deleted == null, cancellationToken);
    }

    public async Task RemoveAsync(VehicleAdditionalEquipment entity, CancellationToken cancellationToken = default)
    {
        var entityToRemove = await _vehicleAdditionalEquipment
            .SingleOrDefaultAsync(vae => vae.Id == entity.Id && vae.Deleted == null, cancellationToken);
        if (entityToRemove is not null)
        {
            _vehicleAdditionalEquipment.Remove(entityToRemove);
        }
    }

    public async Task UpdateAsync(VehicleAdditionalEquipment entity, CancellationToken cancellationToken = default)
    {
        var entityToUpdate = await _vehicleAdditionalEquipment
            .SingleOrDefaultAsync(vae => vae.Id == entity.Id && vae.Deleted == null, cancellationToken);
        if (entityToUpdate is not null)
        {
            _vehicleAdditionalEquipment
                .Entry(entityToUpdate).CurrentValues
                .SetValues(entity);
        }
    }
}
