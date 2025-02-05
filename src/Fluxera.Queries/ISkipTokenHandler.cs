namespace Fluxera.Queries
{
	using Fluxera.Queries.Options;
	using JetBrains.Annotations;

	/// <summary>
	///		A contract for a handler of the $skiptoken values.
	/// </summary>
	[PublicAPI]
	public interface ISkipTokenHandler
	{
		/// <summary>
		///		Gets the URL for the next page of data for service-side paging.
		/// </summary>
		/// <param name="entitySetOptions"></param>
		/// <returns></returns>
		string GetNextLinkUrl(EntitySetOptions entitySetOptions);
	}
}
