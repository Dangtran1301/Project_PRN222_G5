using Project_PRN222_G5.BusinessLogic.DTOs;
using System.Linq.Expressions;

namespace Project_PRN222_G5.BusinessLogic.Extensions;

public static class PagedRequestExtensions
{
    public static Expression<Func<T, bool>>? BuildSearchPredicate<T>(
        this PagedRequest request,
        params Expression<Func<T, string>>[] fields)
    {
        if (string.IsNullOrWhiteSpace(request.Search) || fields.Length == 0)
        {
            return null;
        }

        var parameter = Expression.Parameter(typeof(T), "x");
        var searchText = Expression.Constant(request.Search, typeof(string));

        var body = fields
            .Select(field => Expression.Invoke(field, parameter))
            .Select(invokedField => Expression.Call(
                invokedField,
                typeof(string).GetMethod(nameof(string.Contains), [typeof(string)])!,
                searchText))
            .Aggregate<MethodCallExpression, Expression?>(
                null,
                (current, containsCall) =>
                    current == null ? containsCall : Expression.OrElse(current, containsCall));

        return body != null ? Expression.Lambda<Func<T, bool>>(body, parameter) : null;
    }

    public static IOrderedQueryable<T> ApplyOrdering<T>(
        this IQueryable<T> source,
        string propertyName,
        bool ascending)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
            throw new ArgumentException("Property name is required", nameof(propertyName));

        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.PropertyOrField(parameter, propertyName);
        var lambda = Expression.Lambda(property, parameter);

        var methodName = ascending ? "OrderBy" : "OrderByDescending";

        var result = typeof(Queryable)
            .GetMethods()
            .Single(m => m.Name == methodName && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), property.Type)
            .Invoke(null, [source, lambda]);

        return (IOrderedQueryable<T>)result!;
    }

    public static Func<IQueryable<T>, IOrderedQueryable<T>>? BuildOrderBy<T>(PagedRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Sort)) return null;

        return query => query.ApplyOrdering(
            request.Sort,
            request.SortDir?.ToLower() != "desc"
        );
    }
}