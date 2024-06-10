namespace Fluxera.Queries.AspNetCore.Swagger
{
	using System;
	using System.Linq;
	using Fluxera.Queries.Model;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Options;
	using Swashbuckle.AspNetCore.SwaggerGen;

	[UsedImplicitly]
	internal sealed class ConfigureSwaggerGenOptions : IPostConfigureOptions<SwaggerGenOptions>
	{
		private readonly IEdmTypeProvider typeProvider;

		public ConfigureSwaggerGenOptions(IEdmTypeProvider typeProvider)
		{
			this.typeProvider = typeProvider;
		}

		/// <inheritdoc />
		public void PostConfigure(string name, SwaggerGenOptions options)
		{
			options.CustomSchemaIds(this.SchemaIdSelector);

			options.OperationFilter<QueryOptionsOperationFilter>();
			options.OperationFilter<MetadataCleanupFilter>();

			options.DocumentFilter<ReferenceCorrectionDocumentFilter>();
			options.DocumentFilter<MetadataCleanupFilter>();

			options.SchemaFilter<MetadataEnrichmentSchemaFilter>();
			options.SchemaFilter<EnumSchemaFilter>();
			options.SchemaFilter<EnumerationSchemaFilter>();
			options.SchemaFilter<IgnorePropertiesSchemaFilter>();
		}

		private string SchemaIdSelector(Type modelType)
		{
			if(!modelType.IsConstructedGenericType)
			{
				EdmType edmType = typeProvider.GetByType(modelType);
				if(edmType is EdmComplexType)
				{
					return edmType.Name;
				}

				return modelType.Name.Replace("[]", "Array");
			}

			string prefix = modelType.GetGenericArguments()
									 .Select(this.SchemaIdSelector)
									 .Aggregate((previous, current) => previous + current);

			return prefix + modelType.Name.Split('`').First();
		}
	}
}
