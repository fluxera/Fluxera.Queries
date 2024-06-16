namespace Fluxera.Queries
{
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Queries.Options;
	using JetBrains.Annotations;

	/// <summary>
	///		An abstract base class for an executor service implementation.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	[PublicAPI]
	public abstract class QueryExecutorBase<T, TKey> : IQueryExecutor<T, TKey>
		where T : class
	{
		/// <summary>
		///		Executes the find many query defined by the given <see cref="QueryOptions"/>.
		/// </summary>
		/// <param name="queryOptions"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public abstract Task<QueryResult> ExecuteFindManyAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default);

		/// <summary>
		///		Executes the get query defined by the given ID and <see cref="QueryOptions"/>.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="queryOptions"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public abstract Task<SingleResult> ExecuteGetAsync(TKey id, QueryOptions queryOptions, CancellationToken cancellationToken = default);

		/// <summary>
		///		Executes the count query defined by the given <see cref="QueryOptions"/>.
		/// </summary>
		/// <param name="queryOptions"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public abstract Task<long> ExecuteCountAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default);

		/// <inheritdoc />
		Task<QueryResult> IQueryExecutor.ExecuteFindManyAsync(QueryOptions queryOptions, CancellationToken cancellationToken)
		{
			return this.ExecuteFindManyAsync(queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		Task<SingleResult> IQueryExecutor.ExecuteGetAsync(object id, QueryOptions queryOptions, CancellationToken cancellationToken)
		{
			return this.ExecuteGetAsync((TKey)id, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		Task<long> IQueryExecutor.ExecuteCountAsync(QueryOptions queryOptions, CancellationToken cancellationToken)
		{
			return this.ExecuteCountAsync(queryOptions, cancellationToken);
		}
	}
}
