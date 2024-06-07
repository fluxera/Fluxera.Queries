namespace Fluxera.Queries.AspNetCore
{
	using System;
	using System.Linq;
	using JetBrains.Annotations;
	using Microsoft.AspNetCore.Mvc;
	using System.Text;
	using Fluxera.Queries.Options;
	using Microsoft.AspNetCore.Http;
	using System.Reflection;
	using System.Threading.Tasks;
	using Fluxera.Queries.AspNetCore.ParameterBinding;
	using Fluxera.Guards;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Primitives;

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

			Type entityType = typeof(T);
			DataQuery<T> dataQuery = new DataQuery<T>
			{
				Filter = GetFilterParameterValue(context.Request.Query),
				OrderBy = GetOrderByParameterValue(context.Request.Query)
			};

			IQueryParser parser = context.RequestServices.GetRequiredService<IQueryParser>();
			QueryOptions queryOptions = parser.ParseQueryOptions(entityType, dataQuery.ToString());
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
		public string Filter { get; set; }

		/// <summary>
		///		Gets the '$orderby' query parameter value.
		/// </summary>
		[FromQuery(Name = "$orderby")]
		public string OrderBy { get; set; }

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

			return builder.ToString(0, builder.Length > 0 ? builder.Length - 1 : builder.Length);
		}
	}
}
