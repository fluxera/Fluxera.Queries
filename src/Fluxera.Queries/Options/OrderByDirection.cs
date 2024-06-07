namespace Fluxera.Queries.Options
{
	using JetBrains.Annotations;

	/// <summary>
	///     The valid order by directions.
	/// </summary>
	[PublicAPI]
	public enum OrderByDirection
	{
		/// <summary>
		///     The results are to be filtered by the named property in ascending order.
		/// </summary>
		Ascending = 0,

		/// <summary>
		///     The results are to be filtered by the named property in descending order.
		/// </summary>
		Descending = 1
	}
}
