using System.Linq.Expressions;

namespace Project_PRN222_G5.Domain.Interfaces;

public interface IGenericRepositoryAsync<TEntity> where TEntity : class
{
    #region CRUD
    Task<TEntity> GetByIdAsync(Guid id);

    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

    Task AddAsync(TEntity entity);

    void Update(TEntity entity);

    void Delete(TEntity entity);
    #endregion

    #region bool
    Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>>? criteria = null,
        CancellationToken cancellationToken = default
    );

    Task<int> CountAsync(
        Expression<Func<TEntity, bool>>? criteria = null,
        CancellationToken cancellationToken = default
    );
    #endregion
}