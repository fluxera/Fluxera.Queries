namespace Fluxera.Queries.AspNetCore.Swagger
{
	using Fluxera.Queries.Model;
	using JetBrains.Annotations;
	using Microsoft.OpenApi.Models;
	using Swashbuckle.AspNetCore.SwaggerGen;

	[UsedImplicitly]
	internal sealed class MetadataEnrichmentSchemaFilter : ISchemaFilter
	{
		private readonly IEdmTypeProvider typeProvider;

		public MetadataEnrichmentSchemaFilter(IEdmTypeProvider typeProvider)
		{
			this.typeProvider = typeProvider;
		}

		/// <inheritdoc />
		public void Apply(OpenApiSchema schema, SchemaFilterContext context)
		{
			schema.SetStronglyTypedId(context.Type);
			schema.SetPrimitiveValueObject(context.Type);
			schema.SetEnumeration(context.Type);
			schema.SetComplexType(context.Type, this.typeProvider);
			schema.SetEdmTypeName(context.Type, this.typeProvider);
		}
	}
}
