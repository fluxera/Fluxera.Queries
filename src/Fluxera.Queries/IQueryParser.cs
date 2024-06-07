namespace Fluxera.Queries
{
	using System;
	using Fluxera.Queries.Options;
	using JetBrains.Annotations;

	/// <summary>
	///		The query parser contract.
	/// </summary>
	[PublicAPI]
	public interface IQueryParser
	{
		///  <summary>
		/// 	Parses the given query string options to <see cref="QueryOptions"/>.
		///  </summary>
		///  <param name="entityType">The entity type of the query.</param>
		///  <param name="queryString">The query parameter string of the request.</param>
		///  <returns></returns>
		QueryOptions ParseQueryOptions(Type entityType, string queryString);
	}
}
