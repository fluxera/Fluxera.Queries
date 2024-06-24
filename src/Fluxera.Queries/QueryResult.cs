namespace Fluxera.Queries
{
	using System.Collections.Generic;
	using System.Text.Json.Serialization;
	using Fluxera.Guards;
	using JetBrains.Annotations;

	/// <summary>
	///     A class that represents the multiple results of a query.
	/// </summary>
	[PublicAPI]
	public sealed class QueryResult
	{
		///  <summary>
		/// 		Initializes a new instance of the <see cref="QueryResult"/>.
		///  </summary>
		///  <param name="items"></param>
		///  <param name="totalCount"></param>
		public QueryResult(IReadOnlyCollection<object> items, long? totalCount = null)
		{
			this.Items = Guard.Against.Null(items);
			this.TotalCount = totalCount;
		}

		/// <summary>
		///     Gets the total count of the query (optional).
		/// </summary>
		[JsonPropertyName("@odata.count")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public long? TotalCount { get; }

		/// <summary>
		///     Gets the next link to paged data (optional).
		/// </summary>
		[JsonPropertyName("@odata.nextLink")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string NextLink { get; internal set; }

		/// <summary>
		///     Gets the filtered and paginated results of the query.
		/// </summary>
		[JsonPropertyName("value")]
		public IReadOnlyCollection<object> Items { get; }
	}
}
