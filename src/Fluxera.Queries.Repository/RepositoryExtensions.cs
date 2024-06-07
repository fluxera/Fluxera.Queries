namespace Fluxera.Queries.Repository
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using Fluxera.Queries.Expressions;
	using Fluxera.Queries.Options;
	using Fluxera.Repository;
	using Fluxera.Repository.Query;
	using Fluxera.Repository.Query.Impl;
	using JetBrains.Annotations;

	/// <summary>
	///		Extension methods for use with the <see cref="IReadOnlyRepository{T, TKey}"/>.
	/// </summary>
	[PublicAPI]
	public static class RepositoryExtensions
	{
		/// <summary>
		///		Executes the query defined by the given <see cref="QueryOptions"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="repository"></param>
		/// <param name="queryOptions"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public static async Task<QueryResult<T>> ExecuteQueryAsync<T, TKey>(this IReadOnlyRepository<T, TKey> repository,
			QueryOptions queryOptions, CancellationToken cancellationToken = default)
			where T : AggregateRoot<T, TKey>
			where TKey : IComparable<TKey>, IEquatable<TKey>
		{
			Guard.Against.Null(repository);
			Guard.Against.Null(queryOptions);

			// 1. Build the query options: sorting and paging.
			IQueryOptions<T> options = queryOptions.ToQueryOptions<T>();

			// 2. Build the query predicate.
			Expression<Func<T, bool>> predicate = queryOptions.ToPredicate<T>();

			// 3. Get the total count of the query (optional).
			long? totalCount = null;
			if(queryOptions.Count is not null)
			{
				if(queryOptions.Count.CountValue)
				{
					totalCount = await repository.CountAsync(predicate, cancellationToken);
				}
			}

			// 4. Execute the query.
			IReadOnlyCollection<T> items = await repository.FindManyAsync(predicate, options, cancellationToken);

			return new QueryResult<T>(items, totalCount);
		}

		/// <summary>
		///		Creates a filter predicate from the given <see cref="QueryOptions"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="queryOptions"></param>
		/// <returns></returns>
		public static Expression<Func<T, bool>> ToPredicate<T>(this QueryOptions queryOptions)
		{
			return queryOptions.Filter?.ToExpression<T>() ?? (x => true);
		}

		/// <summary>
		///		Builds <see cref="IQueryOptions{T}"/> from the <see cref="QueryOptions"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="queryOptions"></param>
		/// <returns></returns>
		public static IQueryOptions<T> ToQueryOptions<T>(this QueryOptions queryOptions)
			where T : class
		{
			IQueryOptionsBuilder<T> queryOptionsBuilder = QueryOptionsBuilder.CreateFor<T>();
			return queryOptionsBuilder.Apply(queryOptions).Build();
		}

		/// <summary>
		///		Applies the given <see cref="QueryOptions"/> to the <see cref="IQueryOptionsBuilder{T}"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="builder"></param>
		/// <param name="queryOptions"></param>
		/// <returns></returns>
		public static IQueryOptionsBuilder<T> Apply<T>(this IQueryOptionsBuilder<T> builder, QueryOptions queryOptions)
			where T : class
		{
			// 1. Apply the orderby options.
			if(queryOptions.OrderBy is not null && queryOptions.OrderBy.Properties.Count > 0)
			{
				IReadOnlyCollection<OrderByExpression<T>> expressions = queryOptions.OrderBy.ToExpressions<T>();

				bool isFirstClause = true;

				ISortingOptions<T> sortingOptions = null;

				foreach(OrderByExpression<T> expression in expressions)
				{
					if(isFirstClause)
					{
						sortingOptions = expression.Direction switch
						{
							OrderByDirection.Ascending  => builder.OrderBy(expression.SelectorExpression),
							OrderByDirection.Descending => builder.OrderByDescending(expression.SelectorExpression),
							_                           => throw new InvalidEnumArgumentException($"Unsupported order by direction {expression.Direction}.")
						};
					}
					else
					{
						sortingOptions = expression.Direction switch
						{
							OrderByDirection.Ascending  => sortingOptions.ThenBy(expression.SelectorExpression),
							OrderByDirection.Descending => sortingOptions.ThenByDescending(expression.SelectorExpression),
							_                           => throw new InvalidEnumArgumentException($"Unsupported order by direction {expression.Direction}.")
						};
					}

					isFirstClause = false;
				}
			}

			// 2. Apply paging.
			ISkipTakeOptions<T> skipTakeOptions = queryOptions.Skip is not null
				? builder.Skip(queryOptions.Skip.SkipValue ?? 0)
				: builder.Skip(0);

			if(queryOptions.Top?.TopValue != null)
			{
				skipTakeOptions.Take(queryOptions.Top.TopValue.Value);
			}

			return builder;
		}
	}
}
