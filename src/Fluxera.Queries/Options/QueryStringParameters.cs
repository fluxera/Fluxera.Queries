namespace Fluxera.Queries.Options
{
	using System;
	using System.Text;
	using Fluxera.Queries;
	using Fluxera.Queries.Parsers;

	/// <summary>
	///     A class which contains the query string parameter values.
	/// </summary>
	internal sealed class QueryStringParameters
	{
		public const string FilterParameterName = "$filter";
		public const string OrderByParameterName = "$orderby";

		//public const string SkipParameterName = "$skip";
		//public const string TopParameterName = "$top";
		//public const string SearchParameterName = "$search";
		//public const string SkipTokenParameterName = "$skiptoken";
		//public const string CountParameterName = "$count";
		//public const string SelectParameterName = "$select";

		private QueryStringParameters()
		{
		}

		/// <summary>
		///     Gets the string '$filter' parameter value from the incoming request.
		/// </summary>
		public string Filter { get; set; }

		/// <summary>
		///     Gets the string '$orderby' parameter value from the incoming request.
		/// </summary>
		public string OrderBy { get; set; }

		/// <summary>
		///     Initializes a new instance of the <see cref="QueryStringParameters" /> class.
		/// </summary>
		/// <param name="queryString">The complete query string.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public static QueryStringParameters Create(string queryString)
		{
			Guard.ThrowIfNull(queryString);

			const string filterParameterPrefix = FilterParameterName + "=";
			const string orderByParameterPrefix = OrderByParameterName + "=";

			QueryStringParameters parameters = new QueryStringParameters();

			// Any + signs we want in the data should have been encoded as %2B,
			// so do replace them first otherwise we replace legitimate '+' signs!
			queryString = queryString.Replace('+', ' ');

			if(queryString.Length > 0)
			{
				// Drop the '?'
				queryString = queryString.StartsWith("?") ? queryString.Substring(1) : queryString;

				string[] queryOptions = queryString.Split(SplitCharacter.Ampersand, StringSplitOptions.RemoveEmptyEntries);

				foreach(string queryOption in queryOptions)
				{
					// Decode the chunks to prevent splitting the query on an '&' which is actually part of a string value.
					string decodedQueryOption = Uri.UnescapeDataString(queryOption);

					if(decodedQueryOption.StartsWith(filterParameterPrefix, StringComparison.Ordinal))
					{
						if(decodedQueryOption.Length != filterParameterPrefix.Length)
						{
							parameters.Filter = decodedQueryOption.Substring(filterParameterPrefix.Length);
						}
					}
					else if(decodedQueryOption.StartsWith(orderByParameterPrefix, StringComparison.Ordinal))
					{
						if(decodedQueryOption.Length != orderByParameterPrefix.Length)
						{
							parameters.OrderBy = decodedQueryOption.Substring(orderByParameterPrefix.Length);
						}
					}
				}
			}

			return parameters;
		}

		/// <inheritdoc />
		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();

			if(this.Filter != null)
			{
				builder.Append(FilterParameterName);
				builder.Append('=');
				builder.Append(this.Filter);
				builder.Append('&');
			}

			if(this.OrderBy != null)
			{
				builder.Append(OrderByParameterName);
				builder.Append('=');
				builder.Append(this.OrderBy);
				builder.Append('&');
			}

			//if(this.Skip != null)
			//{
			//	builder.Append(SkipParamName);
			//	builder.Append('=');
			//	builder.Append(this.Skip);
			//	builder.Append('&');
			//}

			//if(this.Top != null)
			//{
			//	builder.Append(TopParamName);
			//	builder.Append('=');
			//	builder.Append(this.Top);
			//	builder.Append('&');
			//}

			//if(this.Search != null)
			//{
			//	builder.Append(SearchParamName);
			//	builder.Append('=');
			//	builder.Append(this.Search);
			//	builder.Append('&');
			//}

			//if(this.SkipToken != null)
			//{
			//	builder.Append(SkipTokenParamName);
			//	builder.Append('=');
			//	builder.Append(this.SkipToken);
			//	builder.Append('&');
			//}

			//if(this.Count != null)
			//{
			//	builder.Append(CountParamName);
			//	builder.Append('=');
			//	builder.Append(this.Count);
			//	builder.Append('&');
			//}

			//if(this.Select != null)
			//{
			//	builder.Append(SelectParamName);
			//	builder.Append('=');
			//	builder.Append(this.Select);
			//	builder.Append('&');
			//}

			return builder.ToString(0, builder.Length > 0 ? builder.Length - 1 : builder.Length);
		}
	}
}
