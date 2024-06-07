namespace Fluxera.Queries.Model
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using Fluxera.Guards;
	using JetBrains.Annotations;

	/// <summary>
	///     The EDM representation of an <see cref="Enum" /> type.
	/// </summary>
	/// <seealso cref="EdmType" />
	[PublicAPI]
	[DebuggerDisplay("{Name}: {ClrType}")]
	public sealed class EdmEnumType : EdmType
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="EdmEnumType" /> type.
		/// </summary>
		/// <param name="clrType">The CLR type of the enum.</param>
		/// <param name="members">The enum members.</param>
		/// <exception cref="ArgumentNullException"></exception>
		internal EdmEnumType(Type clrType, IReadOnlyList<EdmEnumMember> members)
			: base(Guard.Against.Null(clrType).Name, Guard.Against.Null(clrType).FullName, clrType)
		{
			this.Members = Guard.Against.Null(members);
		}

		/// <summary>
		///     Gets the enum members.
		/// </summary>
		public IReadOnlyList<EdmEnumMember> Members { get; }

		/// <summary>
		///     Gets the CLR <see cref="Enum" /> type value for the specified EDM <see cref="Enum" /> member.
		/// </summary>
		/// <param name="value">The <see cref="Enum" /> EDM member name.</param>
		/// <returns>An object containing the CLR Enum value.</returns>
		internal object GetClrValue(string value)
		{
			return Enum.Parse(this.ClrType, value);
		}
	}
}
