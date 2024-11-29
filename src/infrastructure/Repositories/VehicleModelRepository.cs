using Microsoft.EntityFrameworkCore;
using workshopManager.Domain.Abstractions.Interfaces;
using workshopManager.Domain.Entities;
using workshopManager.Infrastructure.Persistence;

namespace workshopManager.Infrastructure.Repositories;

public sealed class VehicleModelRepository(ApplicationDbContext context) : IVehicleModelRepository
{
    private readonly DbSet<VehicleModel> _vehicleModels = context.Set<VehicleModel>();

    public async Task AddAsync(VehicleModel entity, CancellationToken cancellationToken = default)
    {
        await _vehicleModels.AddAsync(entity, cancellationToken);
    }

    public async Task<bool> AlreadyExistsAsync(VehicleModel entity, CancellationToken cancellationToken = default)
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

    public async Task<IEnumerable<VehicleModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _vehicleModels
            .Where(vg => vg.Deleted == null)
            .ToListAsync(cancellationToken) ?? [];
    }

    public async Task<VehicleModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _vehicleModels
            .SingleOrDefaultAsync(vg => vg.Id == id && vg.Deleted == null, cancellationToken);
    }

    public async Task<VehicleModel?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _vehicleModels
            .FirstOrDefaultAsync(ve => EF.Functions.Like(ve.Name, name) && ve.Deleted == null, cancellationToken);
    }

    public async Task RemoveAsync(VehicleModel entity, CancellationToken cancellationToken = default)
    {
        var entityToRemove = await _vehicleModels
            .SingleOrDefaultAsync(vm => vm.Id == entity.Id && vm.Deleted == null, cancellationToken);
        if (entityToRemove is not null)
        {
            _vehicleModels.Remove(entityToRemove);
        }
    }

    public async Task UpdateAsync(VehicleModel entity, CancellationToken cancellationToken = default)
    {
        var entityToUpdate = await _vehicleModels
            .SingleOrDefaultAsync(vm => vm.Id == entity.Id && vm.Deleted == null, cancellationToken);
        if (entityToUpdate is not null)
        {
            _vehicleModels
                .Entry(entityToUpdate).CurrentValues
                .SetValues(entity);
        }
    }
}
