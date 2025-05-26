using Project_PRN222_G5.Infrastructure.Entities.Common;
using Project_PRN222_G5.Infrastructure.Interfaces.Repository;

namespace Project_PRN222_G5.Infrastructure.Interfaces.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IGenericRepositoryAsync<TEntity> Repository<TEntity>()
        where TEntity : BaseEntity;

    Task<int> CompleteAsync();
}