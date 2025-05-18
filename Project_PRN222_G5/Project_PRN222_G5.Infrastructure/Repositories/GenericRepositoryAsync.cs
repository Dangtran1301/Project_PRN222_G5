using Microsoft.EntityFrameworkCore;
using Project_PRN222_G5.Application.Interfaces;
using Project_PRN222_G5.Domain.Common;
using Project_PRN222_G5.Domain.Interfaces;
using System.Linq.Expressions;

namespace Project_PRN222_G5.Infrastructure.Repositories
{
    public class GenericRepositoryAsync<TEntity>(IDbContext context) : IGenericRepositoryAsync<TEntity>
        where TEntity : BaseEntity
    {
        private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id)
                   ?? throw new KeyNotFoundException($"Entity with id {id} not found.");
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
    }
}