using Project_PRN222_G5.DataAccess.Entities.Common;
using Project_PRN222_G5.DataAccess.Interfaces.Data;
using Project_PRN222_G5.DataAccess.Interfaces.Repository;
using Project_PRN222_G5.DataAccess.Interfaces.UnitOfWork;
using Project_PRN222_G5.DataAccess.UnitOfWork.Repositories;

namespace Project_PRN222_G5.DataAccess.UnitOfWork
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