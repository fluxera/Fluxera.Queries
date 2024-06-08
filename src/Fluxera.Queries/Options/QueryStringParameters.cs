namespace Fluxera.Queries.Options
{
	using System;
	using System.Text;
	using Fluxera.Guards;
	using Fluxera.Queries.Parsers;

	/// <summary>
	///     A class which contains the query string parameter values.
	/// </summary>
	internal sealed class QueryStringParameters
	{
		private QueryStringParameters()
		{
		}

		/// <summary>
		///     Gets the string $filter parameter value from the incoming request.
		/// </summary>
		public string Filter { get; private set; }

		/// <summary>
		///     Gets the string $orderby parameter value from the incoming request.
		/// </summary>
		public string OrderBy { get; private set; }

		/// <summary>
		///     Gets the string $skip parameter value from the incoming request.
		/// </summary>
		public string Skip { get; private set; }

		/// <summary>
		///     Gets the string $top parameter value from the incoming request.
		/// </summary>
		public string Top { get; private set; }

		/// <summary>
		///     Gets the string $count parameter value from the incoming request.
		/// </summary>
		public string Count { get; private set; }

		/// <summary>
		///     Gets the string $select parameter value from the incoming request.
		/// </summary>
		public string Select { get; private set; }

		/// <summary>
		///     Initializes a new instance of the <see cref="QueryStringParameters" /> class.
		/// </summary>
		/// <param name="queryString">The complete query string.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public static QueryStringParameters Create(string queryString)
		{
			Guard.Against.Null(queryString);

			const string filterParameterPrefix = ParameterNames.Filter + "=";
			const string orderByParameterPrefix = ParameterNames.OrderBy + "=";
			const string skipParameterPrefix = ParameterNames.Skip + "=";
			const string topParameterPrefix = ParameterNames.Top + "=";
			const string countParameterPrefix = ParameterNames.Count + "=";
			const string selectParameterPrefix = ParameterNames.Select + "=";

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
					else if(decodedQueryOption.StartsWith(skipParameterPrefix, StringComparison.Ordinal))
					{
						if(decodedQueryOption.Length != skipParameterPrefix.Length)
						{
							parameters.Skip = decodedQueryOption.Substring(skipParameterPrefix.Length);
						}
					}
					else if(decodedQueryOption.StartsWith(topParameterPrefix, StringComparison.Ordinal))
					{
						if(decodedQueryOption.Length != topParameterPrefix.Length)
						{
							parameters.Top = decodedQueryOption.Substring(topParameterPrefix.Length);
						}
					}
					else if(decodedQueryOption.StartsWith(countParameterPrefix, StringComparison.Ordinal))
					{
						if(decodedQueryOption.Length != countParameterPrefix.Length)
						{
							parameters.Count = decodedQueryOption.Substring(countParameterPrefix.Length);
						}
					}
					else if(decodedQueryOption.StartsWith(selectParameterPrefix, StringComparison.Ordinal))
					{
						if(decodedQueryOption.Length != selectParameterPrefix.Length)
						{
							parameters.Select = decodedQueryOption.Substring(selectParameterPrefix.Length);
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
				builder.Append(ParameterNames.Filter);
				builder.Append('=');
				builder.Append(this.Filter);
				builder.Append('&');
			}

			if(this.OrderBy != null)
			{
				builder.Append(ParameterNames.OrderBy);
				builder.Append('=');
				builder.Append(this.OrderBy);
				builder.Append('&');
			}

			if(this.Skip != null)
			{
				builder.Append(ParameterNames.Skip);
				builder.Append('=');
				builder.Append(this.Skip);
				builder.Append('&');
			}

			if(this.Top != null)
			{
				builder.Append(ParameterNames.Top);
				builder.Append('=');
				builder.Append(this.Top);
				builder.Append('&');
			}

			if(this.Count != null)
			{
				builder.Append(ParameterNames.Count);
				builder.Append('=');
				builder.Append(this.Count);
				builder.Append('&');
			}

			if(this.Select != null)
			{
				builder.Append(ParameterNames.Select);
				builder.Append('=');
				builder.Append(this.Select);
				builder.Append('&');
			}

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

			return builder.ToString(0, builder.Length > 0 ? builder.Length - 1 : builder.Length);
		}
	}
}
