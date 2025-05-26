using Project_PRN222_G5.Infrastructure.Entities.Common;
using Project_PRN222_G5.Infrastructure.Interfaces.Data;
using Project_PRN222_G5.Infrastructure.Interfaces.Repository;
using Project_PRN222_G5.Infrastructure.Interfaces.UnitOfWork;
using Project_PRN222_G5.Infrastructure.Repositories;

namespace Project_PRN222_G5.Infrastructure.UnitOfWork
{
    public class UnitOfWork(IDbContext context) : IUnitOfWork
    {
        private readonly Dictionary<Type, object> _repositories = new();

        public IGenericRepositoryAsync<TEntity> Repository<TEntity>()
            where TEntity : BaseEntity
        {
            if (_repositories.TryGetValue(typeof(TEntity), out var repository))
            {
                return (IGenericRepositoryAsync<TEntity>)repository;
            }

            var newRepository = new GenericRepositoryAsync<TEntity>(context);
            _repositories.Add(typeof(TEntity), newRepository);
            return newRepository;
        }

        public async Task<int> CompleteAsync()
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose() => context.Dispose();
    }
}