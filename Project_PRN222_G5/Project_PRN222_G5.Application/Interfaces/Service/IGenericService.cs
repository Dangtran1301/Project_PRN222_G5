using Project_PRN222_G5.Application.DTOs;
using Project_PRN222_G5.Domain.Common;
using System.Linq.Expressions;

namespace Project_PRN222_G5.Application.Interfaces.Service;

public interface IGenericService<TE, in TC, in TU, TR>
    where TE : BaseEntity
    where TC : class
    where TU : class
    where TR : class
{
    Task<TR> GetByIdAsync(Guid id);

    Task<IEnumerable<TR>> GetAllAsync();

    Task<PagedResponse> GetPagedAsync(int page, int pageSize, Expression<Func<TE, bool>>? predicate = null);

    Task<TR> CreateAsync(TC request);

    Task<TR> UpdateAsync(Guid id, TU request);

    Task DeleteAsync(Guid id);
}