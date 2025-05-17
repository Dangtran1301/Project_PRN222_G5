using Project_PRN222_G5.Application.Interfaces;
using Project_PRN222_G5.Domain.Common;
using Project_PRN222_G5.Domain.Interfaces;
using System.Linq.Expressions;

namespace Project_PRN222_G5.Application.Services;

public abstract class GenericService<TEntity, TRequest, TResponse>
    (IUnitOfWork unitOfWork) : IGenericService<TEntity, TRequest, TResponse>
    where TEntity : BaseEntity
    where TRequest : class
    where TResponse : class
{
    public async Task<TResponse> GetByIdAsync(Guid id)
    {
        var entity = await unitOfWork.Repository<TEntity>().GetByIdAsync(id);
        return MapToResponse(entity);
    }

    public async Task<IEnumerable<TResponse>> GetAllAsync()
    {
        var entities = await unitOfWork.Repository<TEntity>().GetAllAsync();
        return entities.Select(MapToResponse);
    }

    public async Task<IEnumerable<TResponse>> GetPagedAsync(int page, int pageSize, Expression<Func<TEntity, bool>>? predicate = null)
    {
        var entities = predicate != null
            ? await unitOfWork.Repository<TEntity>().FindAsync(predicate)
            : await unitOfWork.Repository<TEntity>().GetAllAsync();

        return entities
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(MapToResponse)
            .ToList();
    }

    public async Task<TResponse> CreateAsync(TRequest request)
    {
        var entity = MapToEntity(request);
        await unitOfWork.Repository<TEntity>().AddAsync(entity);
        await unitOfWork.CompleteAsync();
        return MapToResponse(entity);
    }

    public async Task<TResponse> UpdateAsync(Guid id, TRequest request)
    {
        var entity = await unitOfWork.Repository<TEntity>().GetByIdAsync(id);
        UpdateEntity(entity, request);
        unitOfWork.Repository<TEntity>().Update(entity);
        await unitOfWork.CompleteAsync();
        return MapToResponse(entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await unitOfWork.Repository<TEntity>().GetByIdAsync(id);
        unitOfWork.Repository<TEntity>().Delete(entity);
        await unitOfWork.CompleteAsync();
    }

    protected abstract TResponse MapToResponse(TEntity entity);

    protected abstract TEntity MapToEntity(TRequest request);

    protected abstract void UpdateEntity(TEntity entity, TRequest request);
}