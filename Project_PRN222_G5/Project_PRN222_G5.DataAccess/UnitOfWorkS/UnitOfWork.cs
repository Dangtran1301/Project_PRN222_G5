using Microsoft.EntityFrameworkCore;
using Project_PRN222_G5.DataAccess.Entities.Common;
using Project_PRN222_G5.DataAccess.Interfaces.Data;
using Project_PRN222_G5.DataAccess.Interfaces.Repository;
using Project_PRN222_G5.DataAccess.Interfaces.Service;
using Project_PRN222_G5.DataAccess.Interfaces.UnitOfWork;
using Project_PRN222_G5.DataAccess.UnitOfWorks.Repositories;

namespace Project_PRN222_G5.DataAccess.UnitOfWorks
{
    public class UnitOfWork(IDbContext context, IDateTimeService dateTimeService, IAuthenticatedUserService authenticatedUserService) : IUnitOfWork
    {
        private readonly Dictionary<Type, object> _repositories = [];
        private bool _disposed = false;

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

        public async Task<int> CompleteAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in context.ChangeTracker.Entries<IBaseAuditable>())
            {
                ApplyAudit(entry.Entity, entry.State);
            }

            return await context.SaveChangesAsync(cancellationToken);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void ApplyAudit(IBaseAuditable entity, EntityState state)
        {
            switch (state)
            {
                case EntityState.Added:
                    entity.CreatedAt = dateTimeService.NowUtc;
                    if (entity.CreatedBy == Guid.Empty)
                    {
                        entity.CreatedBy = Guid.TryParse(authenticatedUserService.UserId, out var userId)
                            ? userId
                            : Guid.Empty;
                    }
                    break;

                case EntityState.Modified:
                    entity.UpdatedAt = dateTimeService.NowUtc;
                    entity.UpdatedBy = Guid.TryParse(authenticatedUserService.UserId, out var updatedBy)
                        ? updatedBy
                        : null;
                    break;
            }
        }
    }
}