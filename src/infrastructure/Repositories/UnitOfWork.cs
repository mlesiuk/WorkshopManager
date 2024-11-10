using Microsoft.EntityFrameworkCore;
using workshopManager.Application.Abstractions.Interfaces;
using workshopManager.Domain.Common;
using workshopManager.Infrastructure.Persistence;

namespace workshopManager.Infrastructure.Repositories;

public sealed class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var username = "system";
        foreach (var entry in context.ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry is null)
            {
                continue;
            }

            if (entry.Entity is null)
            {
                continue;
            }

            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = username;
                    entry.Entity.Created = DateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = username;
                    entry.Entity.LastModified = DateTime.UtcNow;
                    break;
            }
        }
        return await context.SaveChangesAsync(cancellationToken);
    }
}
