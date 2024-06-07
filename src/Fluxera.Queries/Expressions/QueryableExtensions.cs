namespace Fluxera.Queries.Expressions
{
	using System.Linq;
	using JetBrains.Annotations;

	/// <summary>
	///		Extension methods for the <see cref="IQueryable{T}"/> type.
	/// </summary>
	[PublicAPI]
	public static class QueryableExtensions
	{
		//public static QueryResult<T> ToQueryResult<T>(this IQueryable<T> queryable, QueryOptions queryOptions)
		//	//Expression<Func<T, string, bool>> searchPredicate = null)
		//	where T : class
		//{
		//	Guard.Against.Null(queryable);
		//	Guard.Against.Null(queryOptions);

		//	// The order of applying the items to the IQueryable is important.

		//	// 1. Apply query and order.
		//	queryable = queryable.ApplyQuery(queryOptions/*, searchPredicate*/);

		//	//// 2. Optionally get the count of unfiltered items.
		//	long? count = null;
		//	//if(queryOptions.ShowCount)
		//	//{
		//	//	count = queryable.LongCount();
		//	//}

		//	//// 3. Apply paging on the sorted and filtered result.
		//	//queryable = queryable.Paginate(queryOptions);

		//	// 4. Materialize the results.
		//	IList<T> result = queryable.ToList();

		//	return new QueryResult<T>(result, count);
		//}

		///// <summary>
		/////		Applies the filter expression to the queryable.
		///// </summary>
		///// <typeparam name="T"></typeparam>
		///// <param name="queryable"></param>
		///// <param name="filterQueryOption"></param>
		///// <returns></returns>
		//public static IQueryable<T> Where<T>(this IQueryable<T> queryable, FilterQueryOption filterQueryOption)
		//	where T : class
		//{
		//	Guard.Against.Null(queryable);
		//	Guard.Against.Null(filterQueryOption);

		//	if (filterQueryOption.Expression is null)
		//	{
		//		return queryable;
		//	}

		//	Expression<Func<T, bool>> predicate = filterQueryOption.ToExpression<T>();

		//	return queryable.Where(predicate);
		//}

		///// <summary>
		/////		Applies the orderby expression(s) to the queryable.
		///// </summary>
		///// <typeparam name="T"></typeparam>
		///// <param name="queryable"></param>
		///// <param name="orderByQueryOption"></param>
		///// <returns></returns>
		//public static IQueryable<T> OrderBy<T>(this IQueryable<T> queryable, OrderByQueryOption orderByQueryOption)
		//	where T : class
		//{
		//	Guard.Against.Null(queryable);
		//	Guard.Against.Null(orderByQueryOption);

		//	if(orderByQueryOption.Properties.Count == 0)
		//	{
		//		return queryable;
		//	}

		//	OrderByExpression<T>[] expressions = orderByQueryOption.ToExpressions<T>();

		//	bool isFirstClause = true;

		//	foreach(OrderByExpression<T> expression in expressions)
		//	{
		//		// Select correct sort method.
		//		MethodInfo sortMethodInfo = GetQueryableSortMethodInfo(expression.Direction, isFirstClause);

		//		MethodInfo sortMethod = sortMethodInfo.MakeGenericMethod(queryable.ElementType, expression.PropertyExpression.Type);

		//		// Create a new query expression which includes the sort call.
		//		MethodCallExpression queryExpression = Expression.Call(null, sortMethod, queryable.Expression, expression.SelectorExpression);

		//		// Update queryable by using the new query expression.
		//		queryable = queryable.Provider.CreateQuery<T>(queryExpression);

		//		isFirstClause = false;
		//	}

		//	return queryable;
		//}

		//public static IQueryable<T> Search<T>(this IQueryable<T> source, string searchTerm,
		//	Expression<Func<T, string, bool>> searchPredicate)
		//{
		//	Guard.ThrowIfNull(source);

		//	if (searchTerm is null || searchPredicate is null)
		//	{
		//		return source;
		//	}

		//	// We are transforming a generic expression with two arguments into
		//	// a single argument expression that uses a constant value instead of a parameter
		//	// e.g.:
		//	// original:
		//	//                (element, searchText) => element.MyValue.Contains(searchText)
		//	// transformed:
		//	//                var x = "my current search text";
		//	//                element => element.MyValue.Contains(x);
		//	Expression<Func<T, bool>> predicate = ExpressionHelper.BindSecondArgument(searchPredicate, searchTerm);
		//	return source.Where(predicate);
		//}

		//private static IQueryable<T> ApplyQuery<T>(this IQueryable<T> queryable, QueryOptions queryOptions)
		//	//Expression<Func<T, string, bool>> searchPredicate = null)
		//	where T : class
		//{
		//	// 1. Sort to have the correct order for filtering and limiting.
		//	queryable = queryable.OrderBy(queryOptions.OrderBy);

		//	// 2. Filter the items according to the user input.
		//	queryable = queryable.Where(queryOptions.Filter);

		//	return queryable;

		//	//// 3. Handle the "search" parameter.
		//	//return source.Search(queryOptions.Search, searchPredicate);
		//}
	}
}
