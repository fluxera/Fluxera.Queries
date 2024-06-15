namespace Fluxera.Queries.AspNetCore.Options
{
	using JetBrains.Annotations;

	/// <summary>
	///		Build the options for the entity set.
	/// </summary>
	[PublicAPI]
	public interface IEntitySetOptionsBuilder
	{
		/// <summary>
		///		Configure the entity set to always include the @odata.count inline count property.
		/// </summary>
		/// <returns></returns>
		IEntitySetOptionsBuilder AlwaysIncludeCount();
	}
}
