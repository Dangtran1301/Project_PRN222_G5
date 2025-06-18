using Project_PRN222_G5.DataAccess.DTOs;
using Project_PRN222_G5.DataAccess.Exceptions;
using System.Linq.Expressions;

namespace Project_PRN222_G5.DataAccess.Extensions;

public static partial class PagedRequestExtensions
{
    public static Expression<Func<T, bool>>? BuildSearchPredicate<T>(
        this PagedRequest request,
        params Expression<Func<T, string>>[] fields)
    {
        if (string.IsNullOrWhiteSpace(request.Search) || fields.Length == 0)
            return null;

        var parameter = Expression.Parameter(typeof(T), "x");
        var searchText = Expression.Constant(request.Search.ToLower(), typeof(string));

        var body = fields
            .Select(field => Expression.Invoke(field, parameter))
            .Select(invokedField => Expression.Call(
                Expression.Call(invokedField, nameof(string.ToLower), Type.EmptyTypes),
                typeof(string).GetMethod(nameof(string.Contains), [typeof(string)])!,
                searchText))
            .Aggregate<MethodCallExpression, Expression?>(
                null,
                (current, containsCall) =>
                    current == null ? containsCall : Expression.OrElse(current, containsCall));

        return body != null ? Expression.Lambda<Func<T, bool>>(body, parameter) : null;
    }

    private static readonly Dictionary<(Type, string, bool), LambdaExpression> OrderByCache = [];

    public static IOrderedQueryable<T> ApplyOrdering<T>(
        this IQueryable<T> source,
        string propertyName,
        bool ascending)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
        {
            throw new ValidationException(new Dictionary<string, string[]>
            {
                [nameof(propertyName)] = ["Property name is required"]
            });
        }

        var cacheKey = (typeof(T), propertyName, ascending);
        if (!OrderByCache.TryGetValue(cacheKey, out var lambda))
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.PropertyOrField(parameter, propertyName);
            lambda = Expression.Lambda(property, parameter);
            OrderByCache[cacheKey] = lambda;
        }

        var methodName = ascending ? "OrderBy" : "OrderByDescending";
        var result = typeof(Queryable)
            .GetMethods()
            .Single(m => m.Name == methodName && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), lambda.ReturnType)
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

public static partial class ExpressionCombiner
{
    public static Expression<Func<T, bool>> AndAlso<T>(
        this Expression<Func<T, bool>> first,
        Expression<Func<T, bool>> second)
    {
        var parameter = Expression.Parameter(typeof(T));

        var left = ReplaceParameter(first.Body, first.Parameters[0], parameter);
        var right = ReplaceParameter(second.Body, second.Parameters[0], parameter);

        var body = Expression.AndAlso(left, right);
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }

    public static Expression<Func<T, bool>> OrElse<T>(
        this Expression<Func<T, bool>> first,
        Expression<Func<T, bool>> second)
    {
        var parameter = Expression.Parameter(typeof(T));

        var left = ReplaceParameter(first.Body, first.Parameters[0], parameter);
        var right = ReplaceParameter(second.Body, second.Parameters[0], parameter);

        var body = Expression.OrElse(left, right);
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }

    private static Expression ReplaceParameter(Expression expression, ParameterExpression source, ParameterExpression target)
    {
        return new ParameterReplacer(source, target).Visit(expression)!;
    }

    private class ParameterReplacer(ParameterExpression source, ParameterExpression target) : ExpressionVisitor
    {
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == source ? target : base.VisitParameter(node);
        }
    }
}