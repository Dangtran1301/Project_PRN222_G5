using System.Linq.Expressions;

namespace Project_PRN222_G5.Domain.Interfaces;

public interface IGenericRepositoryAsync<TEntity> where TEntity : class
{
    Task<TEntity> GetByIdAsync(Guid id);

    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

    Task AddAsync(TEntity entity);

    void Update(TEntity entity);

    void Delete(TEntity entity);
}