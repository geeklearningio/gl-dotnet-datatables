namespace System.Linq
{
	using Expressions;
	using Reflection;
    using LinqKit;

	public static class QueryableExtensions
	{

        public static IQueryable<TEntity> DynamicWhere<TEntity>(this IQueryable<TEntity> source, string[] searchableColumns, string value)
       where TEntity : class
        {
            var predicate = PredicateBuilder.New<TEntity>();
            ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "p");
            foreach (var propertyName in searchableColumns)
            {
                var property = typeof(TEntity).GetProperty(propertyName);
                Expression instance = Expression.MakeMemberAccess(parameter, property);
                if (property.PropertyType != typeof(string))
                    throw new ArgumentException($"For the moment, you can only search on strings, so not on {propertyName} of type {property.PropertyType}.");
                MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });

                var call = Expression.Call(instance, method, Expression.Constant(value, typeof(string)));
                predicate = predicate.Or(Expression.Lambda<Func<TEntity, bool>>(call, parameter));
            }
            return source.Where(predicate);
        }

        public static IOrderedQueryable<TEntity> DynamicOrderBy<TEntity>(this IQueryable<TEntity> source, string propertyName, bool desc = false, bool then = false)
			where TEntity : class
		{
			return source.DynamicOrderByCore(propertyName, desc ? $"OrderByDescending" : "OrderBy");
		}

		public static IOrderedQueryable<TEntity> DynamicThenBy<TEntity>(this IQueryable<TEntity> source, string propertyName, bool desc = false, bool then = false)
			where TEntity : class
		{
			return source.DynamicOrderByCore(propertyName, desc ? $"ThenByDescending" : "ThenBy");
		}

		private static IOrderedQueryable<TEntity> DynamicOrderByCore<TEntity>(this IQueryable<TEntity> source, string propertyName, string command)
			where TEntity : class
		{
			var type = typeof(TEntity);
			var property = type.GetProperty(propertyName);
			var parameter = Expression.Parameter(type, "p");

			var propertyAccess = Expression.MakeMemberAccess(parameter, property);
			var orderByExpression = Expression.Lambda(propertyAccess, parameter);
			var resultExpression = Expression.Call(
				typeof(Queryable),
				command,
				new Type[] { type, property.PropertyType },
				source.Expression,
				Expression.Quote(orderByExpression));

			return source.Provider.CreateQuery<TEntity>(resultExpression) as IOrderedQueryable<TEntity>;
		}
	}
}
