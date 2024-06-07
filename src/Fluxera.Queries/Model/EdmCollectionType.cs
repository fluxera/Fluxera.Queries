namespace Fluxera.Queries.Model
{
	using System;
	using System.Diagnostics;
	using Fluxera.Guards;
	using JetBrains.Annotations;

	/// <summary>
	///     The EDM representation of a collection type.
	/// </summary>
	/// <seealso cref="EdmType" />
	[PublicAPI]
	[DebuggerDisplay("{FullName}: {ClrType}")]
	public sealed class EdmCollectionType : EdmType
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="EdmCollectionType" /> type.
		/// </summary>
		/// <param name="clrType">The CLR type of the collection type.</param>
		/// <param name="itemType">The EDM type of the collection items.</param>
		internal EdmCollectionType(Type clrType, EdmType itemType)
			: base("Collection", $"Collection({Guard.Against.Null(itemType).FullName})", clrType)
		{
			this.ItemType = Guard.Against.Null(itemType);
		}

		/// <summary>
		///     Gets the <see cref="EdmType" /> type contained in the collection.
		/// </summary>
		public EdmType ItemType { get; }
	}
}
