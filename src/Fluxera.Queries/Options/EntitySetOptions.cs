namespace Fluxera.Queries.Options
{
	using JetBrains.Annotations;

	/// <summary>
	///		The options for an entity set.
	/// </summary>
	[PublicAPI]
	public sealed class EntitySetOptions
	{
		/// <summary>
		///		Flag, indicating if the @odata.count value is always written in
		///		the response, overriding the value provided in the query string.
		/// </summary>
		public bool AlwaysIncludeCount { get; set; }
	}
}
