using Microsoft.EntityFrameworkCore;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Domain.Entities;
using workshopManager.Infrastructure.Persistence;

namespace workshopManager.Infrastructure.Repositories;

public sealed class VehicleEngineRepository(ApplicationDbContext context) : IVehicleEngineRepository
{
	private readonly DbSet<VehicleEngine> _vehicleEngines = context.Set<VehicleEngine>();

	public async Task AddAsync(VehicleEngine entity, CancellationToken cancellationToken = default)
	{
		await _vehicleEngines.AddAsync(entity, cancellationToken);
	}

	public async Task<bool> AlreadyExistsAsync(VehicleEngine vehicleEngine, CancellationToken cancellationToken = default)
	{
		if (await GetByIdAsync(vehicleEngine.Id, cancellationToken) is not null)
		{
			return true;
		}

		if (await GetByNameAsync(vehicleEngine.Name, cancellationToken) is not null)
		{
			return true;
		}

		return false;
    }

    public async Task<IEnumerable<VehicleEngine>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _vehicleEngines
            .Where(ve => ve.Deleted == null)
            .ToListAsync(cancellationToken) ?? [];
    }

    public async Task<VehicleEngine?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
	{
		return await _vehicleEngines
			.SingleOrDefaultAsync(ve => ve.Id == id && ve.Deleted == null, cancellationToken);
	}

    public async Task<VehicleEngine?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _vehicleEngines
            .FirstOrDefaultAsync(ve => EF.Functions.Like(ve.Name, name) && ve.Deleted == null, cancellationToken);
    }

	public async Task RemoveAsync(VehicleEngine vehicleEngine, CancellationToken cancellationToken = default)
    {
        var vehicleEngineToRemove = await _vehicleEngines
            .SingleOrDefaultAsync(ve => ve.Id == vehicleEngine.Id && ve.Deleted == null, cancellationToken);
        if (vehicleEngineToRemove is not null)
        {
            _vehicleEngines.Remove(vehicleEngineToRemove);
        }
    }

	public async Task UpdateAsync(VehicleEngine vehicleEngine, CancellationToken cancellationToken = default)
    {
        var vehicleEngineToUpdate = await _vehicleEngines
            .SingleOrDefaultAsync(ve => ve.Id == vehicleEngine.Id && ve.Deleted == null, cancellationToken);
        if (vehicleEngineToUpdate is not null)
        {
            _vehicleEngines
                .Entry(vehicleEngineToUpdate).CurrentValues
                .SetValues(vehicleEngine);
        }
    }
}
