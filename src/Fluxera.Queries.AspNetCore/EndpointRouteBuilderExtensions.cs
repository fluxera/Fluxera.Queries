namespace Fluxera.Queries.AspNetCore
{
	using System;
	using System.Reflection;
	using System.Text.Json;
	using System.Text.Json.Serialization;
	using System.Threading;
	using System.Threading.Tasks;
	using Fluxera.Enumeration.SystemTextJson;
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
		private static readonly Lazy<JsonSerializerOptions> JsonSerializerOptions = new Lazy<JsonSerializerOptions>(() =>
		{
			JsonSerializerOptions options = new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
				Converters =
				{
					new JsonStringEnumConverter()
				}
			};

			//options.UseSpatial();
			options.UseEnumeration();
			options.UsePrimitiveValueObject();
			options.UseStronglyTypedId();

			return options;
		});

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

			foreach(EntitySetOptions entitySetOptions in options.Value.EntitySetOptions)
			{
				// ReSharper disable once RedundantAssignment
				RouteHandlerBuilder routeHandlerBuilder = null;

				// Map the endpoint for retrieving multiple entities.
				routeHandlerBuilder = routeGroupBuilder
									  .MapGet(entitySetOptions.Name.ToLowerInvariant(), ExecuteFindManyAsync)
									  .WithName($"Find {entitySetOptions.Name}")
									  .WithTags(entitySetOptions.Name)
									  .WithMetadata(entitySetOptions);
				configure?.Invoke(routeHandlerBuilder);

				// Map the endpoint for retrieving an entity by ID.
				routeHandlerBuilder = routeGroupBuilder
									  .MapGet($"{entitySetOptions.Name.ToLowerInvariant()}/{{id:required}}", ExecuteGetAsync)
									  .WithName($"Get {entitySetOptions.Name}")
									  .WithTags(entitySetOptions.Name)
									  .WithMetadata(entitySetOptions);
				configure?.Invoke(routeHandlerBuilder);

				// Map the endpoint for retrieving the count od entities.
				routeHandlerBuilder = routeGroupBuilder
									  .MapGet($"{entitySetOptions.Name.ToLowerInvariant()}/$count", ExecuteCountAsync)
									  .WithName($"Count {entitySetOptions.Name}")
									  .WithTags(entitySetOptions.Name)
									  .WithMetadata(entitySetOptions);
				configure?.Invoke(routeHandlerBuilder);
			}

			return builder;

			static async Task<IResult> ExecuteFindManyAsync(
				HttpContext context,
				[FromQuery(Name = "$filter")] string filter,
				[FromQuery(Name = "$orderby")] string orderby,
				[FromQuery(Name = "$skip")] int? skip,
				[FromQuery(Name = "$top")] int? top,
				[FromQuery(Name = "$count")] bool? count,
				[FromQuery(Name = "$select")] string select,
				CancellationToken cancellationToken = default)
			{
				EntitySetOptions options = GetEntitySetOptions(context);
				DataQuery dataQuery = DataQuery.Create(context, options.EntityType);

				IQueryExecutor queryExecutor = GetQueryExecutor(context, options);
				QueryResult result = await queryExecutor.InternalExecuteFindManyAsync(dataQuery, cancellationToken);

				return Results.Json(result, JsonSerializerOptions.Value, statusCode: 200);
			}

			static async Task<IResult> ExecuteGetAsync(
				HttpContext context,
				[FromRoute] string id,
				[FromQuery(Name = "$select")] string select,
				CancellationToken cancellationToken = default)
			{
				EntitySetOptions options = GetEntitySetOptions(context);
				object identifier = ConvertIdentifier(id, options.IdentifierType);
				DataQuery dataQuery = DataQuery.Create(context, options.EntityType);

				IQueryExecutor queryExecutor = GetQueryExecutor(context, options);
				SingleResult result = await queryExecutor.InternalExecuteGetAsync(identifier, dataQuery, cancellationToken);

				return result.HasValue
					? Results.Json(result.Item, JsonSerializerOptions.Value, statusCode: 200)
					: Results.NotFound();
			}

			static async Task<IResult> ExecuteCountAsync(
				HttpContext context,
				[FromQuery(Name = "$filter")] string filter,
				CancellationToken cancellationToken = default)
			{
				EntitySetOptions options = GetEntitySetOptions(context);
				DataQuery dataQuery = DataQuery.Create(context, options.EntityType);

				IQueryExecutor queryExecutor = GetQueryExecutor(context, options);
				long count = await queryExecutor.InternalExecuteCountAsync(dataQuery, cancellationToken);

				return Results.Text(count.ToString(), statusCode: 200);
			}

			static EntitySetOptions GetEntitySetOptions(HttpContext context)
			{
				EntitySetOptions options = context.GetEndpoint()?.Metadata.GetMetadata<EntitySetOptions>();
				if(options is null)
				{
					throw new InvalidOperationException("The entity set metadata was not available.");
				}

				return options;
			}

			static IQueryExecutor GetQueryExecutor(HttpContext context, EntitySetOptions options)
			{
				Type queryExecutorType = typeof(IQueryExecutor<,>).MakeGenericType(options.EntityType, options.IdentifierType);
				IQueryExecutor queryExecutor = (IQueryExecutor)context.RequestServices.GetRequiredService(queryExecutorType);

				return queryExecutor;
			}

			static object ConvertIdentifier(string id, Type identifierType)
			{
				if(identifierType.IsStronglyTypedId())
				{
					MethodInfo tryParseMethod = identifierType.GetMethod("TryParse", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);

					object[] parameters = [id, null];
					tryParseMethod.Invoke(null, parameters);

					return parameters[1];
				}

				return Convert.ChangeType(id, identifierType);
			}
		}
	}
}
