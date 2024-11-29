using Microsoft.EntityFrameworkCore;
using workshopManager.Domain.Abstractions.Interfaces;
using workshopManager.Domain.Entities;
using workshopManager.Infrastructure.Persistence;

namespace workshopManager.Infrastructure.Repositories;

public sealed class VehicleGearboxRepository(ApplicationDbContext context) : IVehicleGearboxRepository
{
    private readonly DbSet<VehicleGearbox> _vehicleGearboxes = context.Set<VehicleGearbox>();

    public async Task AddAsync(VehicleGearbox entity, CancellationToken cancellationToken = default)
    {
        await _vehicleGearboxes.AddAsync(entity, cancellationToken);
    }

    public async Task<bool> AlreadyExistsAsync(VehicleGearbox entity, CancellationToken cancellationToken = default)
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

    public async Task<IEnumerable<VehicleGearbox>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _vehicleGearboxes
            .Where(vg => vg.Deleted == null)
            .ToListAsync(cancellationToken) ?? [];
    }

    public async Task<VehicleGearbox?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _vehicleGearboxes
            .SingleOrDefaultAsync(vg => vg.Id == id && vg.Deleted == null, cancellationToken);
    }

    public async Task<VehicleGearbox?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _vehicleGearboxes
            .FirstOrDefaultAsync(ve => EF.Functions.Like(ve.Name, name) && ve.Deleted == null, cancellationToken);
    }

    public async Task RemoveAsync(VehicleGearbox entity, CancellationToken cancellationToken = default)
    {
        var entityToRemove = await _vehicleGearboxes
            .SingleOrDefaultAsync(vg => vg.Id == entity.Id && vg.Deleted == null, cancellationToken);
        if (entityToRemove is not null)
        {
            _vehicleGearboxes.Remove(entityToRemove);
        }
    }

    public async Task UpdateAsync(VehicleGearbox entity, CancellationToken cancellationToken = default)
    {
        var entityToUpdate = await _vehicleGearboxes
            .SingleOrDefaultAsync(vg => vg.Id == entity.Id && vg.Deleted == null, cancellationToken);
        if (entityToUpdate is not null)
        {
            _vehicleGearboxes
                .Entry(entityToUpdate).CurrentValues
                .SetValues(entity);
        }
    }
}
