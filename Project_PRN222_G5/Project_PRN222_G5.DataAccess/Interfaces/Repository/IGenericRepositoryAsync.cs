using System.Linq.Expressions;

namespace Project_PRN222_G5.DataAccess.Interfaces.Repository;

public interface IGenericRepositoryAsync<TEntity> where TEntity : class
{
    IQueryable<TEntity> AsQueryable();

    #region CRUD

    Task<TEntity> GetByIdAsync(Guid id);

    Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null);

    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

    Task AddAsync(TEntity entity);

    void Update(TEntity entity);

    void Delete(TEntity entity);

    void RemoveRange(IEnumerable<TEntity> entities);

    #endregion CRUD

    #region bool

    Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>>? criteria = null,
        CancellationToken cancellationToken = default
    );

    #endregion bool

    #region count

    Task<int> CountAsync(
        Expression<Func<TEntity, bool>>? criteria = null,
        CancellationToken cancellationToken = default
    );

    #endregion count
}