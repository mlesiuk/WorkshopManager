using Microsoft.EntityFrameworkCore;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Domain.Entities;
using workshopManager.Domain.Enums;
using workshopManager.Infrastructure.Persistence;

namespace workshopManager.Infrastructure.Repositories;

public sealed class ServiceRequestRepository(ApplicationDbContext context) : IServiceRequestRepository
{
    private readonly DbSet<ServiceRequest> _serviceRequest = context.Set<ServiceRequest>();

    public async Task AddAsync(ServiceRequest entity, CancellationToken cancellationToken = default)
    {
        await _serviceRequest.AddAsync(entity, cancellationToken);
    }

    public async Task<bool> AlreadyExistsAsync(ServiceRequest entity, CancellationToken cancellationToken = default)
    {
        if (await GetByIdAsync(entity.Id, cancellationToken) is not null)
        {
            return true;
        }

        return false;
    }

    public async Task<IEnumerable<ServiceRequest>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _serviceRequest
            .Where(sr => sr.Deleted == null)
            .ToListAsync(cancellationToken) ?? [];
    }

    public async Task<ServiceRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _serviceRequest
            .SingleOrDefaultAsync(sr => sr.Id == id && sr.Deleted == null, cancellationToken);
    }

    public async Task<ServiceRequest?> GetByStatusAsync(ServiceStatus status, CancellationToken cancellationToken = default)
    {
        return await _serviceRequest
            .SingleOrDefaultAsync(sr => sr.Status == status && sr.Deleted == null, cancellationToken);
    }

    public async Task RemoveAsync(ServiceRequest entity, CancellationToken cancellationToken = default)
    {
        var entityToRemove = await _serviceRequest
            .SingleOrDefaultAsync(sr => sr.Id == entity.Id && sr.Deleted == null, cancellationToken);
        if (entityToRemove is not null)
        {
            _serviceRequest.Remove(entityToRemove);
        }
    }

    public async Task UpdateAsync(ServiceRequest entity, CancellationToken cancellationToken = default)
    {
        var entityToUpdate = await _serviceRequest
            .SingleOrDefaultAsync(sr => sr.Id == entity.Id && sr.Deleted == null, cancellationToken);
        if (entityToUpdate is not null)
        {
            _serviceRequest
                .Entry(entityToUpdate).CurrentValues
                .SetValues(entity);
        }
    }
}