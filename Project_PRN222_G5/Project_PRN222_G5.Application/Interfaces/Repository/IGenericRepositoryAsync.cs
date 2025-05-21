using System.Linq.Expressions;

namespace Project_PRN222_G5.Application.Interfaces.Repository;

public interface IGenericRepositoryAsync<TEntity> where TEntity : class
{
    #region CRUD

    Task<TEntity> GetByIdAsync(Guid id);

    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

    Task AddAsync(TEntity entity);

    void Update(TEntity entity);

    void Delete(TEntity entity);

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