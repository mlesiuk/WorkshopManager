using Microsoft.EntityFrameworkCore;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Domain.Entities;
using workshopManager.Infrastructure.Persistence;

namespace workshopManager.Infrastructure.Repositories;

public sealed class VehicleBodyTypeRepository(ApplicationDbContext context) : IVehicleBodyTypeRepository
{
    private readonly DbSet<VehicleBodyType> _vehicleBodyType = context.Set<VehicleBodyType>();

    public async Task AddAsync(VehicleBodyType entity, CancellationToken cancellationToken = default)
    {
        await _vehicleBodyType.AddAsync(entity, cancellationToken);
    }

    public async Task<bool> AlreadyExistsAsync(VehicleBodyType entity, CancellationToken cancellationToken = default)
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

    public async Task<IEnumerable<VehicleBodyType>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _vehicleBodyType
            .Where(vbt => vbt.Deleted == null)
            .ToListAsync(cancellationToken) ?? [];
    }

    public async Task<VehicleBodyType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _vehicleBodyType
            .SingleOrDefaultAsync(vbt => vbt.Id == id && vbt.Deleted == null, cancellationToken);
    }

    public async Task<VehicleBodyType?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _vehicleBodyType
            .FirstOrDefaultAsync(vbt => EF.Functions.Like(vbt.Name, name) && vbt.Deleted == null, cancellationToken);
    }

    public async Task RemoveAsync(VehicleBodyType entity, CancellationToken cancellationToken = default)
    {
        var entityToRemove = await _vehicleBodyType
            .SingleOrDefaultAsync(vbt => vbt.Id == entity.Id && vbt.Deleted == null, cancellationToken);
        if (entityToRemove is not null)
        {
            _vehicleBodyType.Remove(entityToRemove);
        }
    }

    public async Task UpdateAsync(VehicleBodyType entity, CancellationToken cancellationToken = default)
    {
        var entityToUpdate = await _vehicleBodyType
            .SingleOrDefaultAsync(vbt => vbt.Id == entity.Id && vbt.Deleted == null, cancellationToken);
        if (entityToUpdate is not null)
        {
            _vehicleBodyType
                .Entry(entityToUpdate).CurrentValues
                .SetValues(entity);
        }
    }
}
