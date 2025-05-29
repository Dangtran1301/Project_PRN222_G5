using Project_PRN222_G5.BusinessLogic.DTOs;
using Project_PRN222_G5.BusinessLogic.Extensions;
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

    public async Task<PaginationResponse<TR>> GetPagedAsync(
        PagedRequest request,
        Expression<Func<TE, bool>>? predicate = null,
        Func<IQueryable<TE>, IQueryable<TE>>? include = null)
    {
        var searchPredicate = request.BuildSearchPredicate(GetSearchFields());

        Expression<Func<TE, bool>>? finalPredicate = null;

        if (predicate != null && searchPredicate != null)
        {
            var param = Expression.Parameter(typeof(TE));
            var body = Expression.AndAlso(
                Expression.Invoke(predicate, param),
                Expression.Invoke(searchPredicate, param)
            );
            finalPredicate = Expression.Lambda<Func<TE, bool>>(body, param);
        }
        else
        {
            finalPredicate = predicate ?? searchPredicate;
        }

        Func<IQueryable<TE>, IOrderedQueryable<TE>>? orderBy = null;
        if (!string.IsNullOrWhiteSpace(request.Sort))
        {
            orderBy = q => q.ApplyOrdering(request.Sort, request.SortDir?.ToLower() != "desc");
        }

        var (items, totalCount) = await unitOfWork.Repository<TE>().GetPagedAsync(
            request.PageNumber,
            request.PageSize,
            finalPredicate,
            orderBy,
            include
        );

        var data = items.Select(MapToResponse);
        return new PaginationResponse<TR>(data, request.PageNumber, totalCount, request.PageSize);
    }

    public virtual async Task<TR> CreateAsync(TC request)
    {
        validationService.Validate(request);
        var entity = MapToEntity(request);
        await unitOfWork.Repository<TE>().AddAsync(entity);
        await unitOfWork.CompleteAsync();
        return MapToResponse(entity);
    }

    public virtual async Task<TR> UpdateAsync(Guid id, TU request)
    {
        validationService.Validate(request);
        var entity = await unitOfWork.Repository<TE>().GetByIdAsync(id);
        UpdateEntity(entity, request);
        unitOfWork.Repository<TE>().Update(entity);
        await unitOfWork.CompleteAsync();
        return MapToResponse(entity);
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        var entity = await unitOfWork.Repository<TE>().GetByIdAsync(id);
        unitOfWork.Repository<TE>().Delete(entity);
        await unitOfWork.CompleteAsync();
    }

    #endregion CRUD

    #region Mapping

    public abstract TR MapToResponse(TE entity);

    public abstract TE MapToEntity(TC request);

    public abstract void UpdateEntity(TE entity, TU request);

    #endregion Mapping

    protected abstract Expression<Func<TE, string>>[] GetSearchFields();
}