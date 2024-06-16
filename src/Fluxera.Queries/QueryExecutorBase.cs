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
		/// <inheritdoc />
		public abstract Task<QueryResult> ExecuteFindManyAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default);

		/// <inheritdoc />
		public abstract Task<SingleResult> ExecuteGetAsync(TKey id, QueryOptions queryOptions, CancellationToken cancellationToken = default);

		/// <inheritdoc />
		public abstract Task<long> ExecuteCountAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default);

		/// <inheritdoc />
		Task<QueryResult> IQueryExecutor.InternalExecuteFindManyAsync(QueryOptions queryOptions, CancellationToken cancellationToken)
		{
			return this.ExecuteFindManyAsync(queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		Task<SingleResult> IQueryExecutor.InternalExecuteGetAsync(object id, QueryOptions queryOptions, CancellationToken cancellationToken)
		{
			return this.ExecuteGetAsync((TKey)id, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		Task<long> IQueryExecutor.InternalExecuteCountAsync(QueryOptions queryOptions, CancellationToken cancellationToken)
		{
			return this.ExecuteCountAsync(queryOptions, cancellationToken);
		}
	}
}
