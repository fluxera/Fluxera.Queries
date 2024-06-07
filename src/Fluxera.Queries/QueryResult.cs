namespace Fluxera.Queries
{
	using System.Collections.Generic;
	using Fluxera.Guards;
	using JetBrains.Annotations;

	/// <summary>
	///     Represents the results of a query.
	/// </summary>
	/// <typeparam name="T">The item type.</typeparam>
	[PublicAPI]
	public sealed class QueryResult<T>
	{
		/// <summary>
		///		Initializes a new instance of the <see cref="QueryResult{T}"/>.
		/// </summary>
		/// <param name="items"></param>
		/// <param name="totalCount"></param>
		public QueryResult(IReadOnlyCollection<T> items, long? totalCount = null)
		{
			this.Items = Guard.Against.Null(items);
			this.TotalCount = totalCount;
		}

		/// <summary>
		///     Gets the total count of the query (optional).
		/// </summary>
		public long? TotalCount { get; }

		/// <summary>
		///     Gets the filtered and paginated results of the query.
		/// </summary>
		public IReadOnlyCollection<T> Items { get; }
	}
}
