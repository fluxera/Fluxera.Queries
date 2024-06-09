namespace Fluxera.Queries
{
	using Fluxera.Queries.Model;
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
		///  <param name="entitySet">The entity set of the query.</param>
		///  <param name="queryString">The query parameter string of the request.</param>
		///  <returns></returns>
		QueryOptions ParseQueryOptions(EntitySet entitySet, string queryString);
	}
}
