namespace Fluxera.Queries.AspNetCore.Swagger
{
	using Fluxera.Utilities.Extensions;
	using JetBrains.Annotations;
	using Microsoft.OpenApi.Models;
	using Swashbuckle.AspNetCore.SwaggerGen;

	[UsedImplicitly]
	internal sealed class MetadataCleanupFilter : IDocumentFilter, IOperationFilter
	{
		/// <inheritdoc />
		public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
		{
			// Handle the schemas of special custom types.
			foreach((string _, OpenApiSchema schema) in context.SchemaRepository.Schemas)
			{
				schema.Extensions.RemoveMatching(x => x.Key.StartsWith("x-edm-"));

				foreach ((string _, OpenApiSchema property) in schema.Properties)
				{
					property.Extensions.RemoveMatching(x => x.Key.StartsWith("x-edm-"));
				}
			}
		}

		/// <inheritdoc />
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			operation.Extensions.RemoveMatching(x => x.Key.StartsWith("x-edm-"));

			foreach ((string _, OpenApiResponse response) in operation.Responses)
			{
				response.Extensions.RemoveMatching(x => x.Key.StartsWith("x-edm-"));

				foreach ((string _, OpenApiMediaType content) in response.Content)
				{
					content.Extensions.RemoveMatching(x => x.Key.StartsWith("x-edm-"));

					content.Schema.Extensions.RemoveMatching(x => x.Key.StartsWith("x-edm-"));
				}
			}

			foreach (OpenApiParameter parameter in operation.Parameters)
			{
				parameter.Extensions.RemoveMatching(x => x.Key.StartsWith("x-edm-"));
				parameter.Schema.Extensions.RemoveMatching(x => x.Key.StartsWith("x-edm-"));
			}
		}
	}
}
