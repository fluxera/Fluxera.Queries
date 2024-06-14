namespace Fluxera.Queries.Repository
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading;
	using System.Threading.Tasks;
	using AutoMapper;
	using AutoMapper.Extensions.ExpressionMapping;
	using Fluxera.Entity;
	using Fluxera.Guards;
	using Fluxera.Queries;
	using Fluxera.Queries.Options;
	using Fluxera.Repository;
	using Fluxera.Repository.Query;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class MappingRepositoryQueryExecutor<T, TDto, TKey> : QueryExecutorBase<TDto, TKey>
		where T : AggregateRoot<T, TKey>
		where TDto : class
		where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		private readonly IReadOnlyRepository<T, TKey> repository;
		private readonly IMapper mapper;

		public MappingRepositoryQueryExecutor(IReadOnlyRepository<T, TKey> repository, IMapper mapper)
		{
			this.repository = repository;
			this.mapper = mapper;
		}

		/// <inheritdoc />
		public override async Task<QueryResult> ExecuteFindManyAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default)
		{
			Guard.Against.Null(queryOptions);

			// 1. Build the query options: sorting and paging.
			IQueryOptions<TDto> options = queryOptions.ToQueryOptions<TDto>();
			IQueryOptions<T> mappedOptions = null;

			// 2. Build the query predicate.
			Expression<Func<TDto, bool>> predicate = queryOptions.ToPredicate<TDto>();
			Expression<Func<T, bool>> mappedPredicate = this.mapper.MapExpression<Expression<Func<T, bool>>>(predicate);

			// 3. Build the selector expression (optional).
			Expression<Func<TDto, TDto>> selector = queryOptions.ToSelector<TDto>();
			Expression<Func<T, T>> mappedSelector = this.mapper.MapExpression<Expression<Func<T, T>>>(selector);

			// 4. Get the total count of the query (optional).
			long? totalCount = null;
			if(queryOptions.Count is not null)
			{
				if(queryOptions.Count.CountValue)
				{
					totalCount = await this.repository.CountAsync(mappedPredicate, cancellationToken);
				}
			}

			// 5. Execute the find many query.
			IReadOnlyCollection<T> items = selector is null
				? await this.repository.FindManyAsync(mappedPredicate, mappedOptions, cancellationToken)
				: await this.repository.FindManyAsync(mappedPredicate, mappedSelector, mappedOptions, cancellationToken);

			return new QueryResult(items, totalCount);
		}

		/// <inheritdoc />
		public override async Task<SingleResult> ExecuteGetAsync(TKey id, QueryOptions queryOptions, CancellationToken cancellationToken = default)
		{
			Guard.Against.Null(id);
			Guard.Against.Null(queryOptions);

			// 1. Build the selector expression (optional).
			Expression<Func<TDto, TDto>> selector = queryOptions.ToSelector<TDto>();
			Expression<Func<T, T>> mappedSelector = this.mapper.MapExpression<Expression<Func<T, T>>>(selector);

			// 2. Execute the get query.
			T item = selector is null
				? await this.repository.GetAsync(id, cancellationToken)
				: await this.repository.GetAsync(id, mappedSelector, cancellationToken);

			return new SingleResult(item);
		}

		/// <inheritdoc />
		public override async Task<long> ExecuteCountAsync(QueryOptions queryOptions, CancellationToken cancellationToken = default)
		{
			Guard.Against.Null(queryOptions);

			// 1. Build the query predicate.
			Expression<Func<TDto, bool>> predicate = queryOptions.ToPredicate<TDto>();
			Expression<Func<T, bool>> mappedPredicate = this.mapper.MapExpression<Expression<Func<T, bool>>>(predicate);

			// 2. Execute the count query.
			long count = await this.repository.CountAsync(mappedPredicate, cancellationToken);

			return count;
		}
	}
}
