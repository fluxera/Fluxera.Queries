namespace Fluxera.Queries.AspNetCore.Swagger
{
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Options;
	using Swashbuckle.AspNetCore.SwaggerGen;

	[UsedImplicitly]
	internal sealed class ConfigureSwaggerGenOptions : IPostConfigureOptions<SwaggerGenOptions>
	{
		/// <inheritdoc />
		public void PostConfigure(string name, SwaggerGenOptions options)
		{
			options.OperationFilter<QueryOptionsOperationFilter>();
			options.OperationFilter<MetadataCleanupFilter>();

			options.DocumentFilter<ReferenceCorrectionDocumentFilter>();
			options.DocumentFilter<MetadataCleanupFilter>();

			options.SchemaFilter<MetadataEnrichmentSchemaFilter>();
			options.SchemaFilter<EnumSchemaFilter>();
			options.SchemaFilter<EnumerationSchemaFilter>();
		}
	}
}
