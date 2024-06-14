namespace Fluxera.Queries
{
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Queries.Options;
	using JetBrains.Annotations;

	/// <summary>
	///		A contract for a service that executes queries against a data store.
	/// </summary>
	[PublicAPI]
	public interface IQueryExecutor
	{
		/// <summary>
		///		Executes the find many query defined by the given <see cref="QueryOptions"/>.
		/// </summary>
		/// <param name="queryOptions"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		internal Task<QueryResult> InternalExecuteFindManyAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default);

		/// <summary>
		///		Executes the get query defined by the given ID and <see cref="QueryOptions"/>.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="queryOptions"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		internal Task<SingleResult> InternalExecuteGetAsync(object id, QueryOptions queryOptions, CancellationToken cancellationToken = default);

		/// <summary>
		///		Executes the count query defined by the given <see cref="QueryOptions"/>.
		/// </summary>
		/// <param name="queryOptions"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		internal Task<long> InternalExecuteCountAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default);
	}

	///  <summary>
	/// 	A contract for a service that executes queries against a data store.
	///  </summary>
	///  <typeparam name="T"></typeparam>
	///  <typeparam name="TKey"></typeparam>
	[PublicAPI]
	public interface IQueryExecutor<T, in TKey> : IQueryExecutor
		where T : class
	{
		/// <summary>
		///		Executes the find many query defined by the given <see cref="QueryOptions"/>.
		/// </summary>
		/// <param name="queryOptions"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<QueryResult> ExecuteFindManyAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default);

		/// <summary>
		///		Executes the get query defined by the given ID and <see cref="QueryOptions"/>.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="queryOptions"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<SingleResult> ExecuteGetAsync(TKey id, QueryOptions queryOptions, CancellationToken cancellationToken = default);

		/// <summary>
		///		Executes the count query defined by the given <see cref="QueryOptions"/>.
		/// </summary>
		/// <param name="queryOptions"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<long> ExecuteCountAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default);
	}
}
