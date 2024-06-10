namespace Fluxera.Queries.AspNetCore.Swagger
{
	using System;
	using System.Linq;
	using JetBrains.Annotations;
	using Microsoft.OpenApi.Any;
	using Microsoft.OpenApi.Models;
	using Swashbuckle.AspNetCore.SwaggerGen;

	[UsedImplicitly]
	internal sealed class EnumSchemaFilter : ISchemaFilter
	{
		public void Apply(OpenApiSchema schema, SchemaFilterContext context)
		{
			if(context.Type.IsEnum)
			{
				schema.Enum.Clear();
				Enum.GetNames(context.Type)
					.ToList()
					.ForEach(name => schema.Enum.Add(new OpenApiString($"{name}")));
				schema.Type = "string";
				schema.Format = string.Empty;
			}
		}
	}
}
