using Microsoft.EntityFrameworkCore;
using workshopManager.Domain.Abstractions.Interfaces;
using workshopManager.Domain.Entities;
using workshopManager.Infrastructure.Persistence;

namespace workshopManager.Infrastructure.Repositories;

public sealed class VehicleFuelTypeRepository(ApplicationDbContext context) : IVehicleFuelTypeRepository
{
    private readonly DbSet<VehicleFuelType> _vehicleFuelTypes = context.Set<VehicleFuelType>();

    public async Task AddAsync(VehicleFuelType entity, CancellationToken cancellationToken = default)
    {
        await _vehicleFuelTypes.AddAsync(entity, cancellationToken);
    }

    public async Task<bool> AlreadyExistsAsync(VehicleFuelType entity, CancellationToken cancellationToken = default)
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

    public async Task<IEnumerable<VehicleFuelType>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _vehicleFuelTypes
            .Where(vft => vft.Deleted == null)
            .ToListAsync(cancellationToken) ?? [];
    }

    public async Task<VehicleFuelType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _vehicleFuelTypes
            .SingleOrDefaultAsync(vft => vft.Id == id && vft.Deleted == null, cancellationToken);
    }

    public async Task<VehicleFuelType?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _vehicleFuelTypes
            .FirstOrDefaultAsync(ve => EF.Functions.Like(ve.Name, name) && ve.Deleted == null, cancellationToken);
    }

    public async Task RemoveAsync(VehicleFuelType entity, CancellationToken cancellationToken = default)
    {
        var entityToRemove = await _vehicleFuelTypes
            .SingleOrDefaultAsync(vft => vft.Id == entity.Id && vft.Deleted == null, cancellationToken);
        if (entityToRemove is not null)
        {
            _vehicleFuelTypes.Remove(entityToRemove);
        }
    }

    public async Task UpdateAsync(VehicleFuelType entity, CancellationToken cancellationToken = default)
    {
        var entityToUpdate = await _vehicleFuelTypes
            .SingleOrDefaultAsync(vft => vft.Id == entity.Id && vft.Deleted == null, cancellationToken);
        if (entityToUpdate is not null)
        {
            _vehicleFuelTypes
                .Entry(entityToUpdate).CurrentValues
                .SetValues(entity);
        }
    }
}
