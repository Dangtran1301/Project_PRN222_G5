using Project_PRN222_G5.Domain.Common;
using System.Linq.Expressions;

namespace Project_PRN222_G5.Application.Interfaces;

public interface IGenericService<TEntity, TRequest, TResponse>
    where TEntity : BaseEntity
    where TRequest : class
    where TResponse : class
{
    Task<TResponse> GetByIdAsync(Guid id);

    Task<IEnumerable<TResponse>> GetAllAsync();

    Task<IEnumerable<TResponse>> GetPagedAsync(int page, int pageSize, Expression<Func<TEntity, bool>>? predicate = null);

    Task<TResponse> CreateAsync(TRequest request);

    Task<TResponse> UpdateAsync(Guid id, TRequest request);

    Task DeleteAsync(Guid id);
}