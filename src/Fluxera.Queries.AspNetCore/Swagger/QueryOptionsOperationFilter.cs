namespace Fluxera.Queries.AspNetCore.Swagger
{
	using System;
	using System.Linq;
	using JetBrains.Annotations;
	using Microsoft.OpenApi.Extensions;
	using Microsoft.OpenApi.Models;
	using Swashbuckle.AspNetCore.SwaggerGen;

	[UsedImplicitly]
	internal sealed class QueryOptionsOperationFilter : IOperationFilter
	{
		/// <inheritdoc />
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			string operationName = operation.OperationId
				.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
				.FirstOrDefault();

			switch(operationName)
			{
				case "Find":
					AddFindParameters(operation);
					break;
				case "Get":
					AddGetParameters(operation);
					break;
				case "Count":
					AddCountParameters(operation); 
					break;
			}

		}

		private static void AddFindParameters(OpenApiOperation operation)
		{
			AddFilterParameter(operation);
			AddOrderByParameter(operation);
			AddSkipParameter(operation);
			AddTopParameter(operation);
			AddCountParameter(operation);
			AddSelectParameter(operation);
			
			// $search: A global text filter applied to multiple properties to select only a subset of the overall results.
			// $skiptoken: Token used for skipping results.
		}

		private static void AddGetParameters(OpenApiOperation operation)
		{
			AddSelectParameter(operation);
		}

		private static void AddCountParameters(OpenApiOperation operation)
		{
			AddFilterParameter(operation);
		}

		private static void AddFilterParameter(OpenApiOperation operation)
		{
			AddParameter(operation, "$filter", typeof(string), "A filter to select only a subset of the overall results. (<a href='http://docs.oasis-open.org/odata/odata/v4.01/odata-v4.01-part1-protocol.html#_Toc31358948'>Documentation</a>)");
		}

		private static void AddOrderByParameter(OpenApiOperation operation)
		{
			AddParameter(operation, "$orderby", typeof(string), "A comma separated list of properties to sort the result set. (<a href='http://docs.oasis-open.org/odata/odata/v4.01/odata-v4.01-part1-protocol.html#_Toc31358952'>Documentation</a>)");
		}

		private static void AddSkipParameter(OpenApiOperation operation)
		{
			AddParameter(operation, "$skip", typeof(int), "The number of items to skip for paging. (<a href='https://docs.oasis-open.org/odata/odata/v4.01/odata-v4.01-part1-protocol.html#_Toc31358954'>Documentation</a>)");
		}

		private static void AddTopParameter(OpenApiOperation operation)
		{
			AddParameter(operation, "$top", typeof(int), "The number of elements to include from the result set for paging. (<a href='https://docs.oasis-open.org/odata/odata/v4.01/odata-v4.01-part1-protocol.html#_Toc31358953'>Documentation</a>)");
		}

		private static void AddCountParameter(OpenApiOperation operation)
		{
			AddParameter(operation, "$count", typeof(bool), "Whether to include the total number of items in the result set before paging. (<a href='https://docs.oasis-open.org/odata/odata/v4.01/odata-v4.01-part1-protocol.html#_Toc31358955'>Documentation</a>)");
		}

		private static void AddSelectParameter(OpenApiOperation operation)
		{
			AddParameter(operation, "$select", typeof(string), "A comma separated list of properties to select from the result. (<a href='https://docs.oasis-open.org/odata/odata/v4.01/odata-v4.01-part1-protocol.html#_Toc31358942'>Documentation</a>)");
		}

		private static void AddParameter(OpenApiOperation operation, string paramName, Type paramType, string paramDescription)
		{
			OpenApiSchema primitive = paramType.MapTypeToOpenApiPrimitiveType();

			OpenApiParameter param = new OpenApiParameter
			{
				Name = paramName,
				In = ParameterLocation.Query,
				Schema = new OpenApiSchema
				{
					Type = primitive.Type,
					Format = primitive.Format
				},
				Description = paramDescription
			};
			operation.Parameters.Add(param);
		}
	}
}
