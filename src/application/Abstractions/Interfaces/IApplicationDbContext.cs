using Microsoft.EntityFrameworkCore;

namespace workshopManager.Application.Abstractions.Interfaces;

public interface IApplicationDbContextAsync<T> where T : class
{
    DbSet<T> Entities { get; }
    //Task CreateElement(T entity);
    //Task<IEnumerable<T>> GetAllElements();
    //Task<T?> GetElementById(int id);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
