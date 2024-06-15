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
	using Fluxera.Queries.AspNetCore.Options;
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
				.Produces(200, entitySetOptions.ComplexTypeOptions.ClrType);

			return routeHandlerBuilder;

			static async Task<IResult> ExecuteFindManyAsync(HttpContext context,
				CancellationToken cancellationToken = default)
			{
				DataQueriesOptions dataQueriesOptions = context.GetDataQueriesOptions();
				EntitySetOptions entitySetOptions = context.GetEntitySetOptions();
				DataQuery dataQuery = DataQuery.Create(context, entitySetOptions.ComplexTypeOptions.ClrType);

				IQueryExecutor queryExecutor = context.GetQueryExecutor(entitySetOptions);
				QueryResult result = await queryExecutor.InternalExecuteFindManyAsync(dataQuery, cancellationToken);

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
				.Produces(404);

			return routeHandlerBuilder;

			static async Task<IResult> ExecuteGetAsync(HttpContext context,
				[FromRoute] string id,
				CancellationToken cancellationToken = default)
			{
				DataQueriesOptions dataQueriesOptions = GetDataQueriesOptions(context);
				EntitySetOptions entitySetOptions = GetEntitySetOptions(context);
				object identifier = ConvertIdentifier(id, entitySetOptions.KeyType);
				DataQuery dataQuery = DataQuery.Create(context, entitySetOptions.ComplexTypeOptions.ClrType);

				IQueryExecutor queryExecutor = GetQueryExecutor(context, entitySetOptions);
				SingleResult result = await queryExecutor.InternalExecuteGetAsync(identifier, dataQuery, cancellationToken);

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
				.Produces<long>(contentType: "text/plain");

			return routeHandlerBuilder;

			static async Task<IResult> ExecuteCountAsync(HttpContext context,
				CancellationToken cancellationToken = default)
			{
				EntitySetOptions entitySetOptions = GetEntitySetOptions(context);
				DataQuery dataQuery = DataQuery.Create(context, entitySetOptions.ComplexTypeOptions.ClrType);

				IQueryExecutor queryExecutor = GetQueryExecutor(context, entitySetOptions);
				long count = await queryExecutor.InternalExecuteCountAsync(dataQuery, cancellationToken);

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
			Type queryExecutorType = typeof(IQueryExecutor<,>).MakeGenericType(options.ComplexTypeOptions.ClrType, options.KeyType);
			IQueryExecutor queryExecutor = (IQueryExecutor)context.RequestServices.GetRequiredService(queryExecutorType);

			return queryExecutor;
		}

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
						CustomModifier
					}
				}
			};

			//options.UseSpatial();
			jsonSerializerOptions.UseEnumeration();
			jsonSerializerOptions.UsePrimitiveValueObject();
			jsonSerializerOptions.UseStronglyTypedId();

			return jsonSerializerOptions;

			void CustomModifier(JsonTypeInfo jsonTypeInfo)
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
