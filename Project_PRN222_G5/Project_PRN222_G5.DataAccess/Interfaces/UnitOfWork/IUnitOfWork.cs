using Project_PRN222_G5.DataAccess.Entities.Common;
using Project_PRN222_G5.DataAccess.Interfaces.Repository;

namespace Project_PRN222_G5.DataAccess.Interfaces.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IGenericRepositoryAsync<TEntity> Repository<TEntity>()
        where TEntity : BaseEntity;

    Task<int> CompleteAsync(CancellationToken cancellationToken = default);
}