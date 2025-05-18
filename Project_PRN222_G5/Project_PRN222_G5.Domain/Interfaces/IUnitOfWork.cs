using Project_PRN222_G5.Domain.Common;

namespace Project_PRN222_G5.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGenericRepositoryAsync<TEntity> Repository<TEntity>()
        where TEntity : BaseEntity;

    Task<int> CompleteAsync();
}