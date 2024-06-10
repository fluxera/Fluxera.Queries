namespace Fluxera.Queries.AspNetCore.Swagger
{
	using Fluxera.Queries.Model;
	using JetBrains.Annotations;
	using Microsoft.OpenApi.Any;
	using Microsoft.OpenApi.Models;
	using Swashbuckle.AspNetCore.SwaggerGen;

	[UsedImplicitly]
	internal sealed class EnumerationSchemaFilter : ISchemaFilter
	{
		private readonly IEdmTypeProvider typeProvider;

		public EnumerationSchemaFilter(IEdmTypeProvider typeProvider)
		{
			this.typeProvider = typeProvider;
		}

		/// <inheritdoc />
		public void Apply(OpenApiSchema schema, SchemaFilterContext context)
		{
			// Change the enumeration schema.
			if(schema.IsEnumeration())
			{
				schema.Type = "string";
				schema.Format = string.Empty;
				schema.Properties.Clear();
				schema.AdditionalProperties = null;
				schema.AdditionalPropertiesAllowed = true;

				string edmTypeName = schema.GetEdmTypeName();
				EdmEnumType edmType = (EdmEnumType)this.typeProvider.GetByName(edmTypeName);

				foreach(EdmEnumMember edmEnumMember in edmType.Members)
				{
					schema.Enum.Add(new OpenApiString(edmEnumMember.Name));
				}
			}
		}
	}
}
