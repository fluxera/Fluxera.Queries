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
			AddSearchParameter(operation);
			
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
			AddParameter(operation, "$filter", typeof(string), "The $filter system query option restricts the set of items returned. (<a href='http://docs.oasis-open.org/odata/odata/v4.01/odata-v4.01-part1-protocol.html#_Toc31358948'>Documentation</a>)");
		}

		private static void AddOrderByParameter(OpenApiOperation operation)
		{
			AddParameter(operation, "$orderby", typeof(string), "The $orderby System Query option specifies the order in which items are returned from the service. (<a href='http://docs.oasis-open.org/odata/odata/v4.01/odata-v4.01-part1-protocol.html#_Toc31358952'>Documentation</a>)");
		}

		private static void AddSkipParameter(OpenApiOperation operation)
		{
			AddParameter(operation, "$skip", typeof(int), "The $skip system query option specifies a non-negative integer n that excludes the first n items of the queried collection from the result. (<a href='https://docs.oasis-open.org/odata/odata/v4.01/odata-v4.01-part1-protocol.html#_Toc31358954'>Documentation</a>)");
		}

		private static void AddTopParameter(OpenApiOperation operation)
		{
			AddParameter(operation, "$top", typeof(int), "The $top system query option specifies a non-negative integer n that limits the number of items returned from a collection. (<a href='https://docs.oasis-open.org/odata/odata/v4.01/odata-v4.01-part1-protocol.html#_Toc31358953'>Documentation</a>)");
		}

		private static void AddCountParameter(OpenApiOperation operation)
		{
			AddParameter(operation, "$count", typeof(bool), "The $count system query option with a value of true specifies that the total count of items within a collection matching the request be returned along with the result. (<a href='https://docs.oasis-open.org/odata/odata/v4.01/odata-v4.01-part1-protocol.html#_Toc31358955'>Documentation</a>)");
		}

		private static void AddSelectParameter(OpenApiOperation operation)
		{
			AddParameter(operation, "$select", typeof(string), "The $select system query option requests that the service return only the properties explicitly requested by the client. (<a href='https://docs.oasis-open.org/odata/odata/v4.01/odata-v4.01-part1-protocol.html#_Toc31358942'>Documentation</a>)");
		}

		private static void AddSearchParameter(OpenApiOperation operation)
		{
			AddParameter(operation, "$search", typeof(string), "The $search system query option restricts the result to include only those items matching the specified search expression. (<a href='https://docs.oasis-open.org/odata/odata/v4.01/odata-v4.01-part1-protocol.html#_Toc31358956'>Documentation</a>)");
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
