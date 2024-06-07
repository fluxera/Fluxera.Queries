namespace Fluxera.Queries
{
	using System;
	using Fluxera.Guards;
	using Fluxera.Queries.Model;
	using Fluxera.Queries.Options;
	using Fluxera.Utilities.Extensions;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class QueryParser : IQueryParser
	{
		private readonly EdmTypeProvider typeProvider;

		public QueryParser(EdmTypeProvider typeProvider)
		{
			this.typeProvider = typeProvider;
		}

		/// <inheritdoc />
		public QueryOptions ParseQueryOptions(Type entityType, string queryString)
		{
			Guard.Against.Null(entityType);
			Guard.Against.Null(queryString);

			QueryStringParameters parameters = QueryStringParameters.Create(queryString);
			EdmComplexType edmEntityType = (EdmComplexType)this.typeProvider.GetByClrType(entityType);
			EntitySet entitySet = new EntitySet(edmEntityType.Name.ToLowerInvariant().Pluralize(), edmEntityType);

			return new QueryOptions(parameters, entitySet, this.typeProvider);
		}
	}
}
