namespace Fluxera.Queries.AspNetCore.Swagger
{
	using System.Linq;
	using Fluxera.Queries.Model;
	using Fluxera.StronglyTypedId;
	using Fluxera.ValueObject;
	using JetBrains.Annotations;
	using Microsoft.OpenApi.Extensions;
	using Microsoft.OpenApi.Models;
	using Swashbuckle.AspNetCore.SwaggerGen;

	[UsedImplicitly]
	internal sealed class ReferenceCorrectionDocumentFilter : IDocumentFilter
	{
		private readonly IEdmTypeProvider typeProvider;

		public ReferenceCorrectionDocumentFilter(IEdmTypeProvider typeProvider)
		{
			this.typeProvider = typeProvider;
		}

		/// <inheritdoc />
		public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
		{
			// Change the schemas of complex types that use special custom types.
			foreach((string _, OpenApiSchema schema) in context.SchemaRepository.Schemas)
			{
				if(schema.IsComplexType())
				{
					foreach((string _, OpenApiSchema property) in schema.Properties)
					{
						if(!property.Extensions.Any() && property.Reference is not null)
						{
							context.SchemaRepository.Schemas.TryGetValue(property.Reference.Id, out OpenApiSchema referencedSchema);

							if(referencedSchema is not null)
							{
								string edmTypeName = referencedSchema.GetEdmTypeName();
								EdmType edmType = this.typeProvider.GetByName(edmTypeName);

								if(edmType.RedirectedFromType is not null && edmType.RedirectedFromType.IsPrimitiveValueObject())
								{
									OpenApiSchema primitive = edmType.ClrType.MapTypeToOpenApiPrimitiveType();

									property.Type = primitive.Type;
									property.Format = primitive.Format;
									property.Reference = null;
								}
								else if(edmType.RedirectedFromType is not null && edmType.RedirectedFromType.IsStronglyTypedId())
								{
									OpenApiSchema primitive = edmType.ClrType.MapTypeToOpenApiPrimitiveType();

									property.Type = primitive.Type;
									property.Format = primitive.Format;
									property.Reference = null;
								}
							}
						}
					}
				}
			}

			// Remove the schemas of the special types.
			foreach((string schemaName, OpenApiSchema schema) in context.SchemaRepository.Schemas)
			{
				if(schema.IsStronglyTypedId() || schema.IsPrimitiveValueObject())
				{
					swaggerDoc.Components.Schemas.Remove(schemaName);
				}
			}
		}
	}
}
