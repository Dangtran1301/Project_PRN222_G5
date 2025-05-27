using Microsoft.EntityFrameworkCore;
using Project_PRN222_G5.BusinessLogic.DTOs;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service;
using Project_PRN222_G5.BusinessLogic.Interfaces.Validation;
using Project_PRN222_G5.DataAccess.Entities.Common;
using Project_PRN222_G5.DataAccess.Interfaces.UnitOfWork;
using System.Linq.Expressions;

namespace Project_PRN222_G5.BusinessLogic.Services;

public abstract class GenericService<TE, TC, TU, TR>(
    IUnitOfWork unitOfWork,
    IValidationService validationService
    ) : IGenericService<TE, TC, TU, TR>
    where TE : BaseEntity
    where TC : class
    where TU : class
    where TR : class
{
    #region CRUD

    public async Task<TR> GetByIdAsync(Guid id)
    {
        var entity = await unitOfWork.Repository<TE>().GetByIdAsync(id);
        return MapToResponse(entity);
    }

    public async Task<IEnumerable<TR>> GetAllAsync()
    {
        var entities =
            (await unitOfWork.Repository<TE>().GetAllAsync())
            .OrderByDescending(x => x.CreatedAt);
        return entities.Select(MapToResponse);
    }

    public async Task<PagedResponse> GetPagedAsync(int page,
        int pageSize,
        Expression<Func<TE, bool>>? predicate = null,
        Func<IQueryable<TE>, IOrderedQueryable<TE>>? orderBy = null,
        Func<IQueryable<TE>, IQueryable<TE>>? include = null,
        string? searchTerm = null,
        Expression<Func<TE, bool>>? searchPredicate = null)
    {
        var query = unitOfWork.Repository<TE>().AsQueryable();

        if (include != null)
            query = include(query);

        if (predicate != null)
            query = query.Where(predicate);

        if (!string.IsNullOrWhiteSpace(searchTerm) && searchPredicate != null)
            query = query.Where(searchPredicate);

        if (orderBy != null)
            query = orderBy(query);

        var totalCount = await query.CountAsync();

        var pagedItems = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResponse
        {
            Items = pagedItems.Select(MapToResponse),
            TotalCount = totalCount,
            PageNumber = page,
            PageSize = pageSize
        };
    }

    public async Task<TR> CreateAsync(TC request)
    {
        validationService.Validate(request);
        var entity = MapToEntity(request);
        await unitOfWork.Repository<TE>().AddAsync(entity);
        await unitOfWork.CompleteAsync();
        return MapToResponse(entity);
    }

    public async Task<TR> UpdateAsync(Guid id, TU request)
    {
        validationService.Validate(request);
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

    #endregion CRUD

    #region Mapping

    protected abstract TR MapToResponse(TE entity);

    protected abstract TE MapToEntity(TC request);

    protected abstract void UpdateEntity(TE entity, TU request);

    #endregion Mapping
}