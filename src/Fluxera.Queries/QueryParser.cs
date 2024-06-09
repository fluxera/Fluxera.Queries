namespace Fluxera.Queries
{
	using Fluxera.Guards;
	using Fluxera.Queries.Model;
	using Fluxera.Queries.Options;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class QueryParser : IQueryParser
	{
		private readonly IEdmTypeProvider typeProvider;

		public QueryParser(IEdmTypeProvider typeProvider)
		{
			this.typeProvider = typeProvider;
		}

		/// <inheritdoc />
		public QueryOptions ParseQueryOptions(EntitySet entitySet, string queryString)
		{
			Guard.Against.Null(entitySet);
			Guard.Against.Null(queryString);

			QueryStringParameters parameters = QueryStringParameters.Create(queryString);
			return new QueryOptions(parameters, entitySet, this.typeProvider);
		}
	}
}
