namespace Fluxera.Queries.AspNetCore
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Text.Json;
	using System.Text.Json.Serialization;
	using System.Text.Json.Serialization.Metadata;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Enumeration.SystemTextJson;
	using Fluxera.Queries.Options;
	using Fluxera.StronglyTypedId;
	using Fluxera.StronglyTypedId.SystemTextJson;
	using Fluxera.ValueObject.SystemTextJson;
	using JetBrains.Annotations;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Routing;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Options;

	/// <summary>
	/// 	Extension methods for the <see cref="IEndpointRouteBuilder" /> type.
	/// </summary>
	[PublicAPI]
	public static class EndpointRouteBuilderExtensions
	{
		private static JsonSerializerOptions jsonSerializerOptions;

		/// <summary>
		///		Maps the 'Find Entities' endpoint.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="builder"></param>
		/// <returns></returns>
		public static RouteHandlerBuilder MapFindQueryEndpoint<T>(this IEndpointRouteBuilder builder)
			where T : class
		{
			IOptions<DataQueriesOptions> options = builder.ServiceProvider.GetRequiredService<IOptions<DataQueriesOptions>>();
			return builder.MapFindQueryEndpoint(typeof(T), options.Value);
		}

		/// <summary>
		///		Maps the 'Get Entity' endpoint.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="builder"></param>
		/// <returns></returns>
		public static RouteHandlerBuilder MapGetQueryEndpoint<T>(this IEndpointRouteBuilder builder)
			where T : class
		{
			IOptions<DataQueriesOptions> options = builder.ServiceProvider.GetRequiredService<IOptions<DataQueriesOptions>>();
			return builder.MapGetQueryEndpoint(typeof(T), options.Value);
		}

		/// <summary>
		///		Maps the 'Count Entity' endpoint.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="builder"></param>
		/// <returns></returns>
		public static RouteHandlerBuilder MapCountQueryEndpoint<T>(this IEndpointRouteBuilder builder)
			where T : class
		{
			IOptions<DataQueriesOptions> options = builder.ServiceProvider.GetRequiredService<IOptions<DataQueriesOptions>>();
			return builder.MapCountQueryEndpoint(typeof(T), options.Value);
		}

		/// <summary>
		/// 	Maps endpoints for the configured entity sets.
		/// </summary>
		/// <param name="builder">The endpoint route builder.</param>
		/// <param name="configure">An action to configure the endpoint further.</param>
		/// <param name="routePrefix">The route prefix to use.</param>
		/// <returns>The endpoint route builder.</returns>
		public static IEndpointRouteBuilder MapDataQueriesEndpoints(this IEndpointRouteBuilder builder, 
			Action<RouteHandlerBuilder> configure = null, string routePrefix = "api")
		{
			IOptions<DataQueriesOptions> options = builder.ServiceProvider.GetRequiredService<IOptions<DataQueriesOptions>>();

			RouteGroupBuilder routeGroupBuilder = builder.MapGroup(routePrefix);

			foreach(EntitySetOptions entitySetOptions in options.Value.EntitySetOptions.Values)
			{
				Type entityType = entitySetOptions.EntitySet.EdmType.ClrType;

				// Map the endpoint for retrieving multiple entities.
				RouteHandlerBuilder routeHandlerBuilder = routeGroupBuilder.MapFindQueryEndpoint(entityType, options.Value);
				configure?.Invoke(routeHandlerBuilder);

				// Map the endpoint for retrieving an entity by ID.
				routeHandlerBuilder = routeGroupBuilder.MapGetQueryEndpoint(entityType, options.Value);
				configure?.Invoke(routeHandlerBuilder);

				// Map the endpoint for retrieving the count of entities.
				routeHandlerBuilder = routeGroupBuilder.MapCountQueryEndpoint(entityType, options.Value);
				configure?.Invoke(routeHandlerBuilder);
			}

			return builder;
		}

		private static RouteHandlerBuilder MapFindQueryEndpoint(this IEndpointRouteBuilder builder, Type entityType, DataQueriesOptions options)
		{
			EntitySetOptions entitySetOptions = options.GetOptionsByType(entityType);

			// Map the endpoint for retrieving multiple entities.
			RouteHandlerBuilder routeHandlerBuilder = builder
				.MapGet(entitySetOptions.Name, ExecuteFindManyAsync)
				.WithName($"Find {entitySetOptions.Name}")
				.WithTags(entitySetOptions.Name)
				.WithMetadata(options)
				.WithMetadata(entitySetOptions)
				.WithDescription("Retrieves multiple entities by the filter predicate.")
				.WithOpenApi()
				.Produces(200, entitySetOptions.ComplexTypeOptions.ClrType)
				.ProducesProblem(400);

			return routeHandlerBuilder;

			static async Task<IResult> ExecuteFindManyAsync(HttpContext context, CancellationToken cancellationToken = default)
			{
				DataQueriesOptions dataQueriesOptions = context.GetDataQueriesOptions();
				EntitySetOptions entitySetOptions = context.GetEntitySetOptions();
				DataQuery dataQuery = DataQuery.Create(context, entitySetOptions.ComplexTypeOptions.ClrType);

				IQueryExecutor queryExecutor = context.GetQueryExecutor(entitySetOptions);
				QueryResult result = await queryExecutor.ExecuteFindManyAsync(dataQuery, cancellationToken);

				// TODO
				if(entitySetOptions.AllowSkipToken)
				{
					ISkipTokenHandler skipTokenHandler = context.GetSkipTokenHandler();
					result.NextLink = skipTokenHandler.GetNextLinkUrl(entitySetOptions);
				}

				return Results.Json(result, CreateJsonSerializerOptions(dataQueriesOptions), statusCode: 200);
			}
		}

		private static RouteHandlerBuilder MapGetQueryEndpoint(this IEndpointRouteBuilder builder, Type entityType, DataQueriesOptions options)
		{
			EntitySetOptions entitySetOptions = options.GetOptionsByType(entityType);

			// Map the endpoint for retrieving an entity by ID.
			RouteHandlerBuilder routeHandlerBuilder = builder
				.MapGet($"{entitySetOptions.Name}/{{id:required}}", ExecuteGetAsync)
				.WithName($"Get {entitySetOptions.Name}")
				.WithTags(entitySetOptions.Name)
				.WithMetadata(options)
				.WithMetadata(entitySetOptions)
				.WithDescription("Retrieves a single entity by ID.")
				.WithOpenApi()
				.Produces(200, entitySetOptions.ComplexTypeOptions.ClrType)
				.ProducesProblem(400)
				.Produces(404);

			return routeHandlerBuilder;

			static async Task<IResult> ExecuteGetAsync(HttpContext context, [FromRoute] string id, CancellationToken cancellationToken = default)
			{
				DataQueriesOptions dataQueriesOptions = GetDataQueriesOptions(context);
				EntitySetOptions entitySetOptions = GetEntitySetOptions(context);
				object identifier = ConvertIdentifier(id, entitySetOptions.KeyType);
				DataQuery dataQuery = DataQuery.Create(context, entitySetOptions.ComplexTypeOptions.ClrType);

				IQueryExecutor queryExecutor = GetQueryExecutor(context, entitySetOptions);
				SingleResult result = await queryExecutor.ExecuteGetAsync(identifier, dataQuery, cancellationToken);

				return result.HasValue
					? Results.Json(result.Item, CreateJsonSerializerOptions(dataQueriesOptions), statusCode: 200)
					: Results.NotFound();
			}

			static object ConvertIdentifier(string id, Type identifierType)
			{
				if(identifierType.IsStronglyTypedId())
				{
					MethodInfo tryParseMethod = identifierType.GetMethod("TryParse", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);

					object[] parameters = [id, null];
					tryParseMethod?.Invoke(null, parameters);

					return parameters[1];
				}

				return Convert.ChangeType(id, identifierType);
			}
		}

		private static RouteHandlerBuilder MapCountQueryEndpoint(this IEndpointRouteBuilder builder, Type entityType, DataQueriesOptions options)
		{
			EntitySetOptions entitySetOptions = options.GetOptionsByType(entityType);

			// Map the endpoint for retrieving the count of entities.
			RouteHandlerBuilder routeHandlerBuilder = builder
				.MapGet($"{entitySetOptions.Name}/$count", ExecuteCountAsync)
				.WithName($"Count {entitySetOptions.Name}")
				.WithTags(entitySetOptions.Name)
				.WithMetadata(options)
				.WithMetadata(entitySetOptions)
				.WithDescription("Retrieves the count of entities in the data store.")
				.WithOpenApi()
				.Produces<long>(contentType: "text/plain")
				.ProducesProblem(400);

			return routeHandlerBuilder;

			static async Task<IResult> ExecuteCountAsync(HttpContext context, CancellationToken cancellationToken = default)
			{
				EntitySetOptions entitySetOptions = GetEntitySetOptions(context);
				DataQuery dataQuery = DataQuery.Create(context, entitySetOptions.ComplexTypeOptions.ClrType);

				IQueryExecutor queryExecutor = GetQueryExecutor(context, entitySetOptions);
				long count = await queryExecutor.ExecuteCountAsync(dataQuery, cancellationToken);

				return Results.Text(count.ToString(), statusCode: 200);
			}
		}

		private static DataQueriesOptions GetDataQueriesOptions(this HttpContext context)
		{
			DataQueriesOptions options = context.GetEndpoint()?.Metadata.GetMetadata<DataQueriesOptions>();
			if(options is null)
			{
				throw new InvalidOperationException("The data queries options metadata was not available.");
			}

			return options;
		}

		private static EntitySetOptions GetEntitySetOptions(this HttpContext context)
		{
			EntitySetOptions options = context.GetEndpoint()?.Metadata.GetMetadata<EntitySetOptions>();
			if(options is null)
			{
				throw new InvalidOperationException("The entity set metadata was not available.");
			}

			return options;
		}

		private static IQueryExecutor GetQueryExecutor(this HttpContext context, EntitySetOptions options)
		{
			IQueryExecutorFactory queryExecutorFactory = context.RequestServices.GetRequiredService<IQueryExecutorFactory>();
			IQueryExecutor queryExecutor = queryExecutorFactory.Create(options.ComplexTypeOptions.ClrType, options.KeyType);

			return queryExecutor;
		}

		private static ISkipTokenHandler GetSkipTokenHandler(this HttpContext context)
		{
			ISkipTokenHandler skipTokenHandler = context.RequestServices.GetRequiredService<ISkipTokenHandler>();
			return skipTokenHandler;
		}

		//private static string GetNextLinkUrl(this HttpRequest request, EntitySetOptions options)
		//{
		//	string scheme = request.Scheme ?? string.Empty;
		//	string host = request.Host.Value ?? string.Empty;
		//	string pathBase = request.PathBase.Value ?? string.Empty;
		//	string path = request.Path.Value ?? string.Empty;

		//	IDictionary<string, StringValues> query = QueryHelpers.ParseQuery(request.QueryString.Value ?? string.Empty);

		//	int skip = 0;
		//	if(query.TryGetValue("$skip", out StringValues skipValue))
		//	{
		//		skip = skipValue.LastOrDefault().Convert().ToInt();
		//	}

		//	int top = 0;
		//	if(query.TryGetValue("$top", out StringValues topValue))
		//	{
		//		top = topValue.LastOrDefault().Convert().ToInt();
		//	}

		//	// If a max top value is configured, and it has overridden the original value, 
		//	// we calculate the set the next $skip and $top values.
		//	if(options.MaxTop.HasValue && top > options.MaxTop.GetValueOrDefault())
		//	{
		//		// TODO: only continue this would yield a partial page until the total count is reached.

		//		// $top must be the max top value.
		//		top = options.MaxTop.GetValueOrDefault();
		//	}

		//	// $skip must be the provided skip value plus applied top value.
		//	skip += top;

		//	query["$skip"] = skip.ToString();
		//	query["$top"] = top.ToString();
		//	string queryString = QueryHelpers.AddQueryString(string.Empty, query);

		//	// PERF: Calculate string length to allocate correct buffer size for StringBuilder.
		//	int length = scheme.Length + Uri.SchemeDelimiter.Length + host.Length + pathBase.Length + path.Length + queryString.Length;

		//	return new StringBuilder(length)
		//		.Append(scheme)
		//		.Append(Uri.SchemeDelimiter)
		//		.Append(host)
		//		.Append(pathBase)
		//		.Append(path)
		//		.Append(queryString)
		//		.ToString();
		//}

		private static JsonSerializerOptions CreateJsonSerializerOptions(DataQueriesOptions dataQueriesOptions)
		{
			if(jsonSerializerOptions is not null)
			{
				return jsonSerializerOptions;
			}

			jsonSerializerOptions = new JsonSerializerOptions
			{
				WriteIndented = true,
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
				Converters =
				{
					new JsonStringEnumConverter()
				},
				TypeInfoResolver = new DefaultJsonTypeInfoResolver
				{
					Modifiers =
					{
						RemoveIgnoredProperties
					}
				}
			};

			//options.UseSpatial();
			jsonSerializerOptions.UseEnumeration();
			jsonSerializerOptions.UsePrimitiveValueObject();
			jsonSerializerOptions.UseStronglyTypedId();

			return jsonSerializerOptions;

			void RemoveIgnoredProperties(JsonTypeInfo jsonTypeInfo)
			{
				if(jsonTypeInfo.Type.IsClass)
				{
					dataQueriesOptions.EntitySetOptions.TryGetValue(jsonTypeInfo.Type, out EntitySetOptions entitySetOptions);
					dataQueriesOptions.ComplexTypeOptions.TryGetValue(jsonTypeInfo.Type, out ComplexTypeOptions complexTypeOptions);

					complexTypeOptions ??= entitySetOptions?.ComplexTypeOptions;

					if(complexTypeOptions is not null)
					{
						// Remove ignored properties.
						if(jsonTypeInfo.Type == complexTypeOptions.ClrType)
						{
							IList<JsonPropertyInfo> ignoredProperties = new List<JsonPropertyInfo>();

							foreach(PropertyInfo ignoredProperty in complexTypeOptions.IgnoredProperties)
							{
								JsonPropertyInfo jsonPropertyInfo = jsonTypeInfo.Properties.FirstOrDefault(
									x => x.Name.Equals(ignoredProperty.Name, StringComparison.OrdinalIgnoreCase));

								ignoredProperties.Add(jsonPropertyInfo);
							}

							foreach(JsonPropertyInfo ignoredProperty in ignoredProperties)
							{
								jsonTypeInfo.Properties.Remove(ignoredProperty);
							}
						}
					}
				}
			}
		}
	}
}
