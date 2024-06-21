namespace Fluxera.Queries
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using Fluxera.Guards;
	using Fluxera.Queries.Expressions;
	using Fluxera.Queries.Options;
	using JetBrains.Annotations;

	/// <summary>
	///		Extension methods for the <see cref="QueryOptions"/> type.
	/// </summary>
	[PublicAPI]
	public static class QueryOptionsExtensions
	{
		///  <summary>
		/// 	Creates a filter predicate from the given <see cref="QueryOptions"/>.
		///  </summary>
		///  <typeparam name="T"></typeparam>
		///  <param name="queryOptions"></param>
		///  <returns></returns>
		public static Expression<Func<T, bool>> ToPredicate<T>(this QueryOptions queryOptions)
			where T : class
		{
			Guard.Against.Null(queryOptions);

			return queryOptions.ToPredicate<T>(null);
		}

		///  <summary>
		/// 	Creates a filter predicate or a combined predicate for filter and search from the given <see cref="QueryOptions"/>.
		///  </summary>
		///  <typeparam name="T"></typeparam>
		///  <param name="queryOptions"></param>
		///  <param name="searchPredicate"></param>
		///  <returns></returns>
		public static Expression<Func<T, bool>> ToPredicate<T>(this QueryOptions queryOptions, Expression<Func<T, string, bool>> searchPredicate)
			where T : class
		{
			Guard.Against.Null(queryOptions);

			Expression<Func<T, bool>> filter = queryOptions.Filter.ToPredicate<T>();

			if(queryOptions.Search is null || searchPredicate is null)
			{
				return filter;
			}

			Expression<Func<T, bool>> search = queryOptions.Search.ToPredicate(searchPredicate);

			ParameterExpression parameter = Expression.Parameter(typeof(T), "x");

			Expression combined = Expression.AndAlso(filter.Body, search.Body);

			Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(combined, parameter);

			return lambda;
		}

		///  <summary>
		/// 	Creates a search predicate from the given <see cref="FilterQueryOption"/>.
		///  </summary>
		///  <typeparam name="T"></typeparam>
		///  <param name="searchQueryOption"></param>
		///  <param name="searchPredicate"></param>
		///  <returns></returns>
		public static Expression<Func<T, bool>> ToPredicate<T>(this SearchQueryOption searchQueryOption, Expression<Func<T, string, bool>> searchPredicate)
			where T : class
		{
			return searchQueryOption?.ToPredicateExpression(searchPredicate) ?? (x => true);
		}

		/// <summary>
		///		Creates a filter predicate from the given <see cref="FilterQueryOption"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="filterQueryOption"></param>
		/// <returns></returns>
		public static Expression<Func<T, bool>> ToPredicate<T>(this FilterQueryOption filterQueryOption)
			where T : class
		{
			return filterQueryOption?.ToPredicateExpression<T>() ?? (x => true);
		}

		/// <summary>
		///		Creates a selector expression from the given <see cref="QueryOptions"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="queryOptions"></param>
		/// <returns></returns>
		public static Expression<Func<T, T>> ToSelector<T>(this QueryOptions queryOptions)
			where T : class
		{
			Guard.Against.Null(queryOptions);

			return queryOptions.Select.ToSelector<T>();
		}

		/// <summary>
		///		Creates a selector expression from the given <see cref="QueryOptions"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="selectQueryOption"></param>
		/// <returns></returns>
		public static Expression<Func<T, T>> ToSelector<T>(this SelectQueryOption selectQueryOption)
			where T : class
		{
			return selectQueryOption?.ToSelectorExpression<T>() ?? (x => x);
		}

		/// <summary>
		///		Creates the order expressions from the given <see cref="QueryOptions"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="queryOptions"></param>
		/// <returns></returns>
		public static IReadOnlyCollection<OrderByExpression<T>> ToOrderBy<T>(this QueryOptions queryOptions)
			where T : class
		{
			Guard.Against.Null(queryOptions);

			return queryOptions.OrderBy.ToOrderBy<T>();
		}

		/// <summary>
		///		Creates the order expressions from the given <see cref="QueryOptions"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="orderByQueryOption"></param>
		/// <returns></returns>
		public static IReadOnlyCollection<OrderByExpression<T>> ToOrderBy<T>(this OrderByQueryOption orderByQueryOption)
			where T : class
		{
			return orderByQueryOption?.ToOrderByExpressions<T>() ?? [];
		}
	}
}
