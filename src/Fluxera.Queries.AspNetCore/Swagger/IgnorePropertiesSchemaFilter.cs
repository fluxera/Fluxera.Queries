namespace Fluxera.Queries.AspNetCore.Swagger
{
	using System;
	using Fluxera.Queries.Model;
	using Fluxera.Utilities.Extensions;
	using JetBrains.Annotations;
	using Microsoft.OpenApi.Models;
	using Swashbuckle.AspNetCore.SwaggerGen;

	[UsedImplicitly]
	internal sealed class IgnorePropertiesSchemaFilter : ISchemaFilter
	{
		private readonly IEdmTypeProvider typeProvider;

		public IgnorePropertiesSchemaFilter(IEdmTypeProvider typeProvider)
		{
			this.typeProvider = typeProvider;
		}

		/// <inheritdoc />
		public void Apply(OpenApiSchema schema, SchemaFilterContext context)
		{
			// Remove ignored properties.
			if(schema.IsComplexType())
			{
				string edmTypeName = schema.GetEdmTypeName();

				EdmComplexType edmType = (EdmComplexType)this.typeProvider.GetByName(edmTypeName);
				foreach(EdmProperty ignoredProperty in edmType.IgnoredProperties)
				{
					schema.Properties.RemoveMatching(x => x.Key.Equals(ignoredProperty.Name, StringComparison.OrdinalIgnoreCase));
				}
			}
		}
	}
}
