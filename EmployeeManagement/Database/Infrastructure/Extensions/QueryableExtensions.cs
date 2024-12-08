using System.Linq.Expressions;

namespace EmployeeManagement.Database.Infrastructure.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplySorting<T>(
        this IQueryable<T> source,
        string sortBy,
        string sortOrder = "asc")
        {
            if (string.IsNullOrWhiteSpace(sortBy))
                return source;

            // Get the property info for the sortBy field
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = typeof(T).GetProperty(sortBy,
                System.Reflection.BindingFlags.IgnoreCase |
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.Instance);

            if (property == null)
                throw new ArgumentException($"Property '{sortBy}' not found on type '{typeof(T).Name}'");

            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);

            string methodName = sortOrder.ToLower() == "desc" ? "OrderByDescending" : "OrderBy";

            var resultExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { source.ElementType, property.PropertyType },
                source.Expression,
                Expression.Quote(orderByExpression)
            );

            return source.Provider.CreateQuery<T>(resultExpression);
        }
    }
}
