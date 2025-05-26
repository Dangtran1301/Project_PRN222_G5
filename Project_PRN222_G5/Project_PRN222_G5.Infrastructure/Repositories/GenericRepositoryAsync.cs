using Microsoft.EntityFrameworkCore;
using Project_PRN222_G5.Application.Exceptions;
using Project_PRN222_G5.Application.Interfaces.Data;
using Project_PRN222_G5.Application.Interfaces.Repository;
using Project_PRN222_G5.Domain.Common;
using System.Linq.Expressions;

namespace Project_PRN222_G5.Infrastructure.Repositories
{
    public class GenericRepositoryAsync<TEntity>(IDbContext context) : IGenericRepositoryAsync<TEntity>
        where TEntity : BaseEntity
    {
        private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

        public IQueryable<TEntity> AsQueryable()
        {
            return _dbSet.AsNoTracking();
        }

        #region CRUD

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id)
                   ?? throw new ValidationException($"{nameof(TEntity)} with id {id} not found.");
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
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
}