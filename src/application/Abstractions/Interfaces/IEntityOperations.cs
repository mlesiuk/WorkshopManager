using Microsoft.EntityFrameworkCore;

namespace workshopManager.Application.Abstractions.Interfaces;

public interface IEntityOperations<T> where T : class
{
    T CreateElement(T element);
    void UpdateElement(T dbElement, T requestElement);
    string GetElementName(T element);
    Guid GetElementId(T element);
    bool AlreadyExists(T element, DbSet<T> dbSet);
}
