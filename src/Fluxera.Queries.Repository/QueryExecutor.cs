namespace Fluxera.Queries.Repository
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Entity;
	using Fluxera.Queries.Options;
	using Fluxera.Repository;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class QueryExecutor<T, TKey> : QueryExecutorBase<T, TKey>
		where T : AggregateRoot<T, TKey> 
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		private readonly IReadOnlyRepository<T, TKey> repository;

		public QueryExecutor(IReadOnlyRepository<T, TKey> repository)
		{
			this.repository = repository;
		}

		/// <inheritdoc />
		public override Task<QueryResult> ExecuteFindManyAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default)
		{
			return this.repository.ExecuteFindManyAsync(queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		public override Task<SingleResult> ExecuteGetAsync(TKey id, QueryOptions queryOptions, CancellationToken cancellationToken = default)
		{
			return this.repository.ExecuteGetAsync(id, queryOptions, cancellationToken);
		}

		/// <inheritdoc />
		public override Task<long> ExecuteCountAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default)
		{
			return this.repository.ExecuteCountAsync(queryOptions, cancellationToken);
		}
	}
}
