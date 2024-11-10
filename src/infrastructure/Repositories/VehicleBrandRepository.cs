using Microsoft.EntityFrameworkCore;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Domain.Entities;
using workshopManager.Infrastructure.Persistence;

namespace workshopManager.Infrastructure.Repositories;

public sealed class VehicleBrandRepository(ApplicationDbContext context) : IVehicleBrandRepository
{
	private readonly DbSet<VehicleBrand> _vehicleBrands = context.Set<VehicleBrand>();

	public async Task AddAsync(VehicleBrand entity, CancellationToken cancellationToken = default)
	{
		await _vehicleBrands.AddAsync(entity, cancellationToken);
	}

	public async Task<bool> AlreadyExistsAsync(VehicleBrand vehicleBrand, CancellationToken cancellationToken = default)
	{
		if (await GetByIdAsync(vehicleBrand.Id, cancellationToken) is not null)
		{
			return true;
		}

		if (await GetByNameAsync(vehicleBrand.Name, cancellationToken) is not null)
		{
			return true;
		}

		return false;
	}

	public async Task<VehicleBrand?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
	{
		return await _vehicleBrands
			.SingleOrDefaultAsync(vb => vb.Id == id, cancellationToken);
	}

    public async Task<VehicleBrand?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _vehicleBrands
            .FirstOrDefaultAsync(vb => EF.Functions.Like(vb.Name, name), cancellationToken);
    }
}
