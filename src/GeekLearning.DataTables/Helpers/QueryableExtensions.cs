namespace System.Linq
{
	using Expressions;
	using Reflection;

	public static class QueryableExtensions
	{
		public static IOrderedQueryable<TEntity> DynamicWhere<TEntity>(this IQueryable<TEntity> source, string propertyName, string value)
			where TEntity : class
		{
			ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "p");
			Expression instance = Expression.MakeMemberAccess(parameter, typeof(TEntity).GetProperty(propertyName));
			MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });

			var call = Expression.Call(instance, method, Expression.Constant(value, typeof(string)));

			var resultExpression = Expression.Call(
				typeof(Queryable),
				"Where",
				new Type[] { typeof(TEntity) },
				source.Expression,
				Expression.Quote(Expression.Lambda<Func<TEntity, bool>>(call, parameter))
			);

			return source.Provider.CreateQuery<TEntity>(resultExpression) as IOrderedQueryable<TEntity>;
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
