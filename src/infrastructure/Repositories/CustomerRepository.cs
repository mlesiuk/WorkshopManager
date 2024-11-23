using Microsoft.EntityFrameworkCore;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Domain.Entities;
using workshopManager.Infrastructure.Persistence;

namespace workshopManager.Infrastructure.Repositories;

public sealed class CustomerRepository(ApplicationDbContext context) : ICustomerRepository
{
    private readonly DbSet<Customer> _customer = context.Set<Customer>();

    public async Task AddAsync(Customer entity, CancellationToken cancellationToken = default)
    {
        await _customer.AddAsync(entity, cancellationToken);
    }

    public async Task<bool> AlreadyExistsAsync(Customer entity, CancellationToken cancellationToken = default)
    {
        if (await GetByIdAsync(entity.Id, cancellationToken) is not null)
        {
            return true;
        }

        if (await GetByEmailAsync(entity.Email, cancellationToken) is not null)
        {
            return true;
        }

        return false;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _customer
            .Where(c => c.Deleted == null)
            .ToListAsync(cancellationToken) ?? [];
    }

    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _customer
            .SingleOrDefaultAsync(c => c.Id == id && c.Deleted == null, cancellationToken);
    }

    public async Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _customer
            .FirstOrDefaultAsync(c => EF.Functions.Like(c.Email, email) && c.Deleted == null, cancellationToken);
    }

    public async Task RemoveAsync(Customer entity, CancellationToken cancellationToken = default)
    {
        var entityToRemove = await _customer
            .SingleOrDefaultAsync(c => c.Id == entity.Id && c.Deleted == null, cancellationToken);
        if (entityToRemove is not null)
        {
            _customer.Remove(entityToRemove);
        }
    }

    public async Task UpdateAsync(Customer entity, CancellationToken cancellationToken = default)
    {
        var entityToUpdate = await _customer
            .SingleOrDefaultAsync(c => c.Id == entity.Id && c.Deleted == null, cancellationToken);
        if (entityToUpdate is not null)
        {
            _customer
                .Entry(entityToUpdate).CurrentValues
                .SetValues(entity);
        }
    }
}
