using Microsoft.EntityFrameworkCore;
using Project_PRN222_G5.DataAccess.Entities.Common;
using Project_PRN222_G5.DataAccess.Interfaces.Data;
using Project_PRN222_G5.DataAccess.Interfaces.Repository;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Project_PRN222_G5.DataAccess.Repositories
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

        public async Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedAsync(
            int page,
            int pageSize,
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
        {
            var query = _dbSet.AsQueryable();

            if (include is not null)
            {
                query = include(query);
            }

            if (predicate is not null)
            {
                query = query.Where(predicate);
            }

            var countQuery = query;
            var totalCount = await countQuery.CountAsync();

            if (orderBy is not null)
            {
                query = orderBy(query);
            }

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return (items, totalCount);
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