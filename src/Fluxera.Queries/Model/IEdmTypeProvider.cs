namespace Fluxera.Queries.Model
{
	using System;
	using JetBrains.Annotations;

	/// <summary>
	///		A contract for the EDM model provider.
	/// </summary>
	[PublicAPI]
	public interface IEdmTypeProvider
	{
		///  <summary>
		/// 	Gets or creates an EDM type for the given CLR type.
		///  </summary>
		///  <param name="clrType">The CLR type.</param>
		///  <returns>The corresponding EDM type.</returns>
		EdmType GetByType(Type clrType);

		/// <summary>
		///		Gets an EDM type for the given EDM type name.
		/// </summary>
		/// <param name="edmTypeName">The full EDM type name.</param>
		/// <returns></returns>
		EdmType GetByName(string edmTypeName);
	}
}
