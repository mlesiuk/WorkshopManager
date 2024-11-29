using Microsoft.EntityFrameworkCore;
using workshopManager.Domain.Abstractions.Interfaces;
using workshopManager.Domain.Entities;
using workshopManager.Infrastructure.Persistence;

namespace workshopManager.Infrastructure.Repositories;

public sealed class VehiclePropulsionRepository(ApplicationDbContext context) : IVehiclePropulsionRepository
{
    private readonly DbSet<VehiclePropulsion> _vehiclePropulsions = context.Set<VehiclePropulsion>();

    public async Task AddAsync(VehiclePropulsion entity, CancellationToken cancellationToken = default)
    {
        await _vehiclePropulsions.AddAsync(entity, cancellationToken);
    }

    public async Task<bool> AlreadyExistsAsync(VehiclePropulsion entity, CancellationToken cancellationToken = default)
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

    public async Task<IEnumerable<VehiclePropulsion>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _vehiclePropulsions
            .Where(vg => vg.Deleted == null)
            .ToListAsync(cancellationToken) ?? [];
    }

    public async Task<VehiclePropulsion?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _vehiclePropulsions
            .SingleOrDefaultAsync(vg => vg.Id == id && vg.Deleted == null, cancellationToken);
    }

    public async Task<VehiclePropulsion?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _vehiclePropulsions
            .FirstOrDefaultAsync(ve => EF.Functions.Like(ve.Name, name) && ve.Deleted == null, cancellationToken);
    }

    public async Task RemoveAsync(VehiclePropulsion entity, CancellationToken cancellationToken = default)
    {
        var entityToRemove = await _vehiclePropulsions
            .SingleOrDefaultAsync(vp => vp.Id == entity.Id && vp.Deleted == null, cancellationToken);
        if (entityToRemove is not null)
        {
            _vehiclePropulsions.Remove(entityToRemove);
        }
    }

    public async Task UpdateAsync(VehiclePropulsion entity, CancellationToken cancellationToken = default)
    {
        var entityToUpdate = await _vehiclePropulsions
            .SingleOrDefaultAsync(vp => vp.Id == entity.Id && vp.Deleted == null, cancellationToken);
        if (entityToUpdate is not null)
        {
            _vehiclePropulsions
                .Entry(entityToUpdate).CurrentValues
                .SetValues(entity);
        }
    }
}
