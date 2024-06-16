// ReSharper disable PossibleNullReferenceException
namespace Fluxera.Queries.AspNetCore
{
	using System;
	using System.Linq;
	using JetBrains.Annotations;
	using Microsoft.AspNetCore.Mvc;
	using System.Text;
	using Microsoft.AspNetCore.Http;
	using System.Reflection;
	using System.Threading.Tasks;
	using Fluxera.Queries.AspNetCore.ParameterBinding;
	using Fluxera.Guards;
	using Fluxera.Queries.Model;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Options;
	using Microsoft.Extensions.Primitives;
	using Fluxera.Queries.Options;

	/// <summary>
	///		A class that should be used in controller actions and endpoint delegates
	///		to bind and parse the query string parameters.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[PublicAPI]
	public sealed class DataQuery<T> : DataQuery, IParameterBinder<DataQuery<T>>
		where T : class
	{
		/// <inheritdoc />
		public static ValueTask<DataQuery<T>> BindAsync(HttpContext context, ParameterInfo parameter)
		{
			if(context.Request.Method != "GET")
			{
				throw new InvalidOperationException("The data queries ca only be used for GET requests.");
			}

			DataQuery<T> dataQuery = new DataQuery<T>
			{
				Filter = GetFilterParameterValue(context.Request.Query),
				OrderBy = GetOrderByParameterValue(context.Request.Query),
				Skip = GetSkipParameterValue(context.Request.Query),
				Top = GetTopParameterValue(context.Request.Query),
				Count = GetCountParameterValue(context.Request.Query),
				Select = GetSelectParameterValue(context.Request.Query)
			};

			IOptions<DataQueriesOptions> dataQueryOptions = context.RequestServices.GetRequiredService<IOptions<DataQueriesOptions>>();
			EntitySet entitySet = dataQueryOptions.Value.GetByType<T>();

			IQueryParser parser = context.RequestServices.GetRequiredService<IQueryParser>();
			QueryOptions queryOptions = parser.ParseQueryOptions(entitySet, dataQuery.ToString());
			dataQuery.QueryOptions = queryOptions;

			return ValueTask.FromResult(dataQuery);
		}
	}

	/// <summary>
	///		A base class for the query options container.
	/// </summary>
	[PublicAPI]
	public abstract class DataQuery
	{
		/// <summary>
		///		Gets the '$filter' query parameter value.
		/// </summary>
		[FromQuery(Name = "$filter")]
		public string Filter { get; internal set; }

		/// <summary>
		///		Gets the '$orderby' query parameter value.
		/// </summary>
		[FromQuery(Name = "$orderby")]
		public string OrderBy { get; internal set; }

		/// <summary>
		///		Gets the '$skip' query parameter value.
		/// </summary>
		[FromQuery(Name = "$skip")]
		public int? Skip { get; internal set; }

		/// <summary>
		///		Gets the '$top' query parameter value.
		/// </summary>
		[FromQuery(Name = "$top")]
		public int? Top { get; internal set; }

		/// <summary>
		///		Gets the '$count' query parameter value.
		/// </summary>
		[FromQuery(Name = "$count")]
		public bool? Count { get; internal set; }

		/// <summary>
		///		Gets the '$count' query parameter value.
		/// </summary>
		[FromQuery(Name = "$select")]
		public string Select { get; internal set; }

		/// <summary>
		///		Converts the data query to parsed query options.
		/// </summary>
		/// <returns></returns>
		public QueryOptions ToQueryOptions()
		{
			return this.QueryOptions;
		}

		/// <summary>
		///		Converts the data query to parsed query options.
		/// </summary>
		/// <param name="query"></param>
		public static implicit operator QueryOptions(DataQuery query)
		{
			return query?.ToQueryOptions();
		}

		internal static DataQuery Create(HttpContext context, Type entityType)
		{
			if(context.Request.Method != "GET")
			{
				throw new InvalidOperationException("The data queries ca only be used for GET requests.");
			}

			Type dataQueryType = typeof(DataQuery<>).MakeGenericType(entityType);
			
			DataQuery dataQuery = (DataQuery)Activator.CreateInstance(dataQueryType);

			dataQuery.Filter = GetFilterParameterValue(context.Request.Query);
			dataQuery.OrderBy = GetOrderByParameterValue(context.Request.Query);
			dataQuery.Skip = GetSkipParameterValue(context.Request.Query);
			dataQuery.Top = GetTopParameterValue(context.Request.Query);
			dataQuery.Count = GetCountParameterValue(context.Request.Query);
			dataQuery.Select = GetSelectParameterValue(context.Request.Query);

			IOptions<DataQueriesOptions> dataQueryOptions = context.RequestServices.GetRequiredService<IOptions<DataQueriesOptions>>();
			EntitySet entitySet = dataQueryOptions.Value.GetByType(entityType);

			IQueryParser parser = context.RequestServices.GetRequiredService<IQueryParser>();
			QueryOptions queryOptions = parser.ParseQueryOptions(entitySet, dataQuery.ToString());
			dataQuery.QueryOptions = queryOptions;

			return dataQuery;
		}

		internal QueryOptions QueryOptions { get; set; }

		internal static string GetFilterParameterValue(IQueryCollection collection)
		{
			Guard.Against.Null(collection);

			if(collection.TryGetValue(ParameterNames.Filter, out StringValues value))
			{
				return value.Single();
			}

			return null;
		}

		internal static string GetOrderByParameterValue(IQueryCollection collection)
		{
			Guard.Against.Null(collection);

			if(collection.TryGetValue(ParameterNames.OrderBy, out StringValues value))
			{
				return value.Single();
			}

			return null;
		}

		internal static int? GetSkipParameterValue(IQueryCollection collection)
		{
			Guard.Against.Null(collection);

			if(collection.TryGetValue(ParameterNames.Skip, out StringValues value))
			{
				string skipParameterValue = value.Single();
				int.TryParse(skipParameterValue, out int skipValue);
				return skipValue;
			}

			return null;
		}

		internal static int? GetTopParameterValue(IQueryCollection collection)
		{
			Guard.Against.Null(collection);

			if(collection.TryGetValue(ParameterNames.Top, out StringValues value))
			{
				string topParameterValue = value.Single();
				int.TryParse(topParameterValue, out int topValue);
				return topValue;
			}

			return null;
		}

		internal static bool? GetCountParameterValue(IQueryCollection collection)
		{
			Guard.Against.Null(collection);

			if(collection.TryGetValue(ParameterNames.Count, out StringValues value))
			{
				string countParameterValue = value.Single();
				bool.TryParse(countParameterValue, out bool countValue);
				return countValue;
			}

			return null;
		}

		internal static string GetSelectParameterValue(IQueryCollection collection)
		{
			Guard.Against.Null(collection);

			if(collection.TryGetValue(ParameterNames.Select, out StringValues value))
			{
				return value.Single();
			}

			return null;
		}

		/// <inheritdoc />
		public override string ToString()
		{
			StringBuilder builder = new StringBuilder("?");

			if(this.Filter is not null)
			{
				builder.Append("$filter=");
				builder.Append(this.Filter);
				builder.Append('&');
			}

			if(this.OrderBy is not null)
			{
				builder.Append("$orderby=");
				builder.Append(this.OrderBy);
				builder.Append('&');
			}

			if(this.Skip is not null)
			{
				builder.Append("$skip=");
				builder.Append(this.Skip);
				builder.Append('&');
			}

			if(this.Top is not null)
			{
				builder.Append("$top=");
				builder.Append(this.Top);
				builder.Append('&');
			}

			if(this.Count is not null)
			{
				builder.Append("$count=");
				builder.Append(this.Count);
				builder.Append('&');
			}

			if(this.Select is not null)
			{
				builder.Append("$select=");
				builder.Append(this.Select);
				builder.Append('&');
			}

			return builder.ToString(0, builder.Length > 0 ? builder.Length - 1 : builder.Length);
		}
	}
}
