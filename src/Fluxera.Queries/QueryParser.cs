namespace Fluxera.Queries
{
	using Fluxera.Guards;
	using Fluxera.Queries.Model;
	using Fluxera.Queries.Options;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Options;

	[UsedImplicitly]
	internal sealed class QueryParser : IQueryParser
	{
		private readonly IEdmTypeProvider typeProvider;
		private readonly DataQueriesOptions options;

		public QueryParser(IEdmTypeProvider typeProvider, IOptions<DataQueriesOptions> options)
		{
			this.typeProvider = typeProvider;
			this.options = options.Value;
		}

		/// <inheritdoc />
		public QueryOptions ParseQueryOptions(EntitySet entitySet, string queryString)
		{
			Guard.Against.Null(entitySet);
			Guard.Against.Null(queryString);

			EntitySetOptions entitySetOptions = this.options.GetOptionsByType(entitySet.EdmType.ClrType) ?? new EntitySetOptions();

			QueryStringParameters parameters = QueryStringParameters.Create(queryString, entitySetOptions);

			return new QueryOptions(parameters, entitySet, entitySetOptions, this.typeProvider);
		}
	}
}
