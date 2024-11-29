using Microsoft.EntityFrameworkCore;
using workshopManager.Domain.Abstractions.Interfaces;
using workshopManager.Domain.Entities;
using workshopManager.Infrastructure.Persistence;

namespace workshopManager.Infrastructure.Repositories;

public sealed class WorkerRepository(ApplicationDbContext context) : IWorkerRepository
{
    private readonly DbSet<Worker> _workers = context.Set<Worker>();


    public async Task<Worker?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _workers
            .SingleOrDefaultAsync(w => w.Id == id && w.Deleted == null, cancellationToken);
    }
}
