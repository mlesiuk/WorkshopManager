using Microsoft.EntityFrameworkCore;
using workshopManager.Domain.Abstractions.Interfaces;
using workshopManager.Domain.Entities;
using workshopManager.Infrastructure.Persistence;

namespace workshopManager.Infrastructure.Repositories;

public sealed class VehicleRepository(ApplicationDbContext context) : IVehicleRepository
{
    private readonly DbSet<Vehicle> _vehicle = context.Set<Vehicle>();
    public async Task AddAsync(Vehicle entity, CancellationToken cancellationToken = default)
    {
        await _vehicle.AddAsync(entity, cancellationToken);
    }

    public async Task<bool> AlreadyExistsAsync(Vehicle entity, CancellationToken cancellationToken = default)
    {
        if (await GetByIdAsync(entity.Id, cancellationToken) is not null)
        {
            return true;
        }

        return false;
    }

    public async Task<IEnumerable<Vehicle>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _vehicle
            .Where(v => v.Deleted == null)
            .ToListAsync(cancellationToken) ?? [];
    }

    public async Task<Vehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _vehicle
            .SingleOrDefaultAsync(v => v.Id == id && v.Deleted == null, cancellationToken);
    }

    public async Task RemoveAsync(Vehicle entity, CancellationToken cancellationToken = default)
    {
        var entityToRemove = await _vehicle
            .SingleOrDefaultAsync(v => v.Id == entity.Id && v.Deleted == null, cancellationToken);
        if (entityToRemove is not null)
        {
            _vehicle.Remove(entityToRemove);
        }
    }

    public async Task UpdateAsync(Vehicle entity, CancellationToken cancellationToken = default)
    {
        var entityToUpdate = await _vehicle
            .SingleOrDefaultAsync(v => v.Id == entity.Id && v.Deleted == null, cancellationToken);
        if (entityToUpdate is not null)
        {
            _vehicle
                .Entry(entityToUpdate).CurrentValues
                .SetValues(entity);
        }
    }
}
