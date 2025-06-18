using Microsoft.EntityFrameworkCore;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service;
using Project_PRN222_G5.BusinessLogic.Interfaces.Validation;
using Project_PRN222_G5.DataAccess.DTOs;
using Project_PRN222_G5.DataAccess.Entities.Common;
using Project_PRN222_G5.DataAccess.Exceptions;
using Project_PRN222_G5.DataAccess.Extensions;
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

    public async Task<TR> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await unitOfWork.Repository<TE>().AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return entity == null ? throw new ValidationException($"{typeof(TE).Name} with id {id} not found.") : MapToResponse(entity);
    }

    public async Task<IEnumerable<TR>> GetAllAsync(string? sort = null, bool ascending = true)
    {
        var query = unitOfWork.Repository<TE>().AsQueryable();
        if (!string.IsNullOrWhiteSpace(sort))
        {
            query = query.ApplyOrdering(sort, ascending);
        }
        var entities = await query.ToListAsync();
        return entities.Select(MapToResponse);
    }

    public async Task<PaginationResponse<TR>> GetPagedAsync(
        PagedRequest request,
        Expression<Func<TE, bool>>? predicate = null,
        Func<IQueryable<TE>, IQueryable<TE>>? include = null)
    {
        var searchPredicate = request.BuildSearchPredicate(GetSearchFields());

        var finalPredicate = CombinePredicates(predicate, searchPredicate);

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
        if (!validationService.TryValidate(request, out var errors))
            throw new ValidationException(errors);
        var entity = MapToEntity(request);
        await unitOfWork.Repository<TE>().AddAsync(entity);
        await unitOfWork.CompleteAsync();
        return MapToResponse(entity);
    }

    public virtual async Task<TR> UpdateAsync(Guid id, TU request)
    {
        if (!validationService.TryValidate(request, out var errors))
            throw new ValidationException(errors);
        var entity = await unitOfWork.Repository<TE>().GetByIdAsync(id, true);
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

    private static readonly Dictionary<Type, Expression<Func<object, string>>[]> SearchFieldsCache = [];

    protected virtual Expression<Func<TE, string>>[] GetSearchFields()
    {
        if (SearchFieldsCache.TryGetValue(typeof(TE), out var fields))
            return [.. fields.Cast<Expression<Func<TE, string>>>()];

        var searchFields = DefineSearchFields();
        SearchFieldsCache[typeof(TE)] = [.. searchFields.Cast<Expression<Func<object, string>>>()];
        return searchFields;
    }

    protected virtual Expression<Func<TE, string>>[] DefineSearchFields()
    {
        throw new NotImplementedException("Define search fields in derived classes.");
    }
    private static Expression<Func<TE, bool>>? CombinePredicates(Expression<Func<TE, bool>>? predicate, Expression<Func<TE, bool>>? searchPredicate)
    {
        if (predicate == null) return searchPredicate;
        if (searchPredicate == null) return predicate;
        return predicate.AndAlso(searchPredicate);
    }
}