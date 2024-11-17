using Microsoft.EntityFrameworkCore;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Domain.Entities;
using workshopManager.Infrastructure.Persistence;

namespace workshopManager.Infrastructure.Repositories;

public sealed class VehicleGenerationRepository(ApplicationDbContext context) : IVehicleGenerationRepository
{
    private readonly DbSet<VehicleGeneration> _vehicleGenerations = context.Set<VehicleGeneration>();

    public async Task AddAsync(VehicleGeneration entity, CancellationToken cancellationToken = default)
    {
        await _vehicleGenerations.AddAsync(entity, cancellationToken);
    }

    public async Task<bool> AlreadyExistsAsync(VehicleGeneration entity, CancellationToken cancellationToken = default)
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

    public async Task<IEnumerable<VehicleGeneration>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _vehicleGenerations
            .Where(vg => vg.Deleted == null)
            .ToListAsync(cancellationToken) ?? [];
    }

    public async Task<VehicleGeneration?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _vehicleGenerations
            .SingleOrDefaultAsync(vg => vg.Id == id && vg.Deleted == null, cancellationToken);
    }

    public async Task<VehicleGeneration?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _vehicleGenerations
            .FirstOrDefaultAsync(ve => EF.Functions.Like(ve.Name, name) && ve.Deleted == null, cancellationToken);
    }

    public async Task RemoveAsync(VehicleGeneration entity, CancellationToken cancellationToken = default)
    {
        var entityToRemove = await _vehicleGenerations
            .SingleOrDefaultAsync(vg => vg.Id == entity.Id && vg.Deleted == null, cancellationToken);
        if (entityToRemove is not null)
        {
            _vehicleGenerations.Remove(entityToRemove);
        }
    }

    public async Task UpdateAsync(VehicleGeneration entity, CancellationToken cancellationToken = default)
    {
        var entityToUpdate = await _vehicleGenerations
            .SingleOrDefaultAsync(vg => vg.Id == entity.Id && vg.Deleted == null, cancellationToken);
        if (entityToUpdate is not null)
        {
            _vehicleGenerations
                .Entry(entityToUpdate).CurrentValues
                .SetValues(entity);
        }
    }
}
