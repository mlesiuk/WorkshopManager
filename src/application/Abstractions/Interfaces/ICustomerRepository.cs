using workshopManager.Domain.Entities;

namespace workshopManager.Application.Abstractions.Interfaces;

public interface ICustomerRepository
{
    public Task AddAsync(Customer entity, CancellationToken cancellationToken = default);
    public Task<bool> AlreadyExistsAsync(Customer entity, CancellationToken cancellationToken = default);
    public Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<Customer?> GetByEmailAsync(string name, CancellationToken cancellationToken = default);
    public Task RemoveAsync(Customer entity, CancellationToken cancellationToken = default);
    public Task UpdateAsync(Customer entity, CancellationToken cancellationToken = default);
}
