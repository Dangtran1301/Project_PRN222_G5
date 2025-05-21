using Project_PRN222_G5.Application.DTOs;
using Project_PRN222_G5.Application.Interfaces.Service;
using Project_PRN222_G5.Domain.Common;
using Project_PRN222_G5.Domain.Interfaces;
using System.Linq.Expressions;

namespace Project_PRN222_G5.Application.Services;

public abstract class GenericService<TE, TC, TU, TR>
    (IUnitOfWork unitOfWork) : IGenericService<TE, TC, TU, TR>
    where TE : BaseEntity
    where TC : class
    where TU : class
    where TR : class
{
    public async Task<TR> GetByIdAsync(Guid id)
    {
        var entity = await unitOfWork.Repository<TE>().GetByIdAsync(id);
        return MapToResponse(entity);
    }

    public async Task<IEnumerable<TR>> GetAllAsync()
    {
        var entities = await unitOfWork.Repository<TE>().GetAllAsync();
        return entities.Select(MapToResponse);
    }

    public async Task<PagedResponse> GetPagedAsync(int page, int pageSize, Expression<Func<TE, bool>>? predicate = null)
    {
        var entities = predicate != null
            ? await unitOfWork.Repository<TE>().FindAsync(predicate)
            : await unitOfWork.Repository<TE>().GetAllAsync();

        var totalCount = entities.Count();
        var pagedItems = entities
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(MapToResponse)
            .ToList();

        return new PagedResponse
        {
            Items = pagedItems,
            TotalCount = totalCount,
            PageNumber = page,
            PageSize = pageSize
        };
    }

    public virtual async Task<TR> CreateAsync(TC request)
    {
        var entity = MapToEntity(request);
        await unitOfWork.Repository<TE>().AddAsync(entity);
        await unitOfWork.CompleteAsync();
        return MapToResponse(entity);
    }

    public virtual async Task<TR> UpdateAsync(Guid id, TU request)
    {
        var entity = await unitOfWork.Repository<TE>().GetByIdAsync(id);
        UpdateEntity(entity, request);
        unitOfWork.Repository<TE>().Update(entity);
        await unitOfWork.CompleteAsync();
        return MapToResponse(entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await unitOfWork.Repository<TE>().GetByIdAsync(id);
        unitOfWork.Repository<TE>().Delete(entity);
        await unitOfWork.CompleteAsync();
    }

    protected abstract TR MapToResponse(TE entity);

    protected abstract TE MapToEntity(TC request);

    protected abstract void UpdateEntity(TE entity, TU request);
}