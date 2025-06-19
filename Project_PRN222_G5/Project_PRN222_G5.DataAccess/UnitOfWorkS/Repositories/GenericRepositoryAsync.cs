using Microsoft.EntityFrameworkCore;
using Project_PRN222_G5.DataAccess.Entities.Common;
using Project_PRN222_G5.DataAccess.Interfaces.Data;
using Project_PRN222_G5.DataAccess.Interfaces.Repository;
using System.Linq.Expressions;

namespace Project_PRN222_G5.DataAccess.UnitOfWorks.Repositories;

public class GenericRepositoryAsync<TEntity>(IDbContext context) : IGenericRepositoryAsync<TEntity>
    where TEntity : BaseEntity
{
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    public IQueryable<TEntity> AsQueryable()
    {
        return _dbSet.AsNoTracking();
    }

    #region CRUD

    public async Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default, bool track = false)
    {
        var query = track ? _dbSet : _dbSet.AsNoTracking();
        return await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken) ?? default!;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    public async Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();
        if (include != null)
            query = include(query);
        if (predicate != null)
            query = query.Where(predicate);

        var count = await query.CountAsync(cancellationToken);
        query = orderBy != null ? orderBy(query) : query.OrderBy(e => e.Id);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return (items, count);
    }

    #endregion CRUD

    #region bool

    public async Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>>? criteria = null,
        CancellationToken cancellationToken = default)
    {
        if (criteria is null)
        {
            return await _dbSet.AnyAsync(cancellationToken);
        }

        return await _dbSet.AnyAsync(criteria, cancellationToken);
    }

    #endregion bool

    #region count

    public async Task<int> CountAsync(
        Expression<Func<TEntity, bool>>? criteria = null,
        CancellationToken cancellationToken = default)
    {
        if (criteria is null)
        {
            return await _dbSet.CountAsync(cancellationToken);
        }

        return await _dbSet.CountAsync(criteria, cancellationToken);
    }

    #endregion count
}