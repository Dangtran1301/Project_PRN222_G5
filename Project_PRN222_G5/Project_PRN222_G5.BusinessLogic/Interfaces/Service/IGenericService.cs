using Project_PRN222_G5.DataAccess.DTOs;
using Project_PRN222_G5.DataAccess.Entities.Common;
using Project_PRN222_G5.DataAccess.Interfaces.Mapping;
using System.Linq.Expressions;

namespace Project_PRN222_G5.BusinessLogic.Interfaces.Service;

public interface IGenericService<TE, in TC, in TU, TR> : IMapper<TE, TC, TU, TR>
    where TE : BaseEntity
    where TC : class
    where TU : class
    where TR : class
{
    #region CRUD

    Task<TR> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IEnumerable<TR>> GetAllAsync(CancellationToken cancellationToken = default, string? sort = null, bool ascending = true);

    Task<PaginationResponse<TR>> GetPagedAsync(
        PagedRequest request,
        Expression<Func<TE, bool>>? predicate = null,
        Func<IQueryable<TE>, IQueryable<TE>>? include = null,
        CancellationToken cancellationToken = default);

    Task<TR> CreateAsync(TC request, CancellationToken cancellationToken = default);

    Task<TR> UpdateAsync(Guid id, TU request, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    #endregion CRUD
}