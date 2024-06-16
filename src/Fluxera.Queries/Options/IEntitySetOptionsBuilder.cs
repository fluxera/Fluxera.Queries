namespace Fluxera.Queries.Options
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

		/// <summary>
		///		Adds a metadata entry for the entity set.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		IEntitySetOptionsBuilder WithMetadata(string key, object value);
	}
}
