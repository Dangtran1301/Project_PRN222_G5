using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Project_PRN222_G5.DataAccess.Interfaces.Data;

public interface IDbContext : IDisposable
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;

    EntityEntry Entry(object entity);

    ChangeTracker ChangeTracker { get; }

    public DatabaseFacade DatabaseFacade { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}