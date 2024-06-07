namespace Fluxera.Queries.Model
{
	using System;
	using System.Diagnostics;
	using JetBrains.Annotations;

	/// <summary>
	///     The EDM representation of an enum member.
	/// </summary>
	[PublicAPI]
	[DebuggerDisplay("{Value}: {Name}")]
	public sealed class EdmEnumMember
	{
		/// <summary>
		///		Initializes a new instance of the <see cref="EdmEnumMember"/> type.
		/// </summary>
		/// <param name="name">The name of the enum member.</param>
		/// <param name="value">The value of the enum member.</param>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		internal EdmEnumMember(string name, int value)
		{
			this.Name = Guard.ThrowIfNullOrWhiteSpace(name);
			this.Value = value;
		}

		/// <summary>
		///     Gets the name of the enum member.
		/// </summary>
		public string Name { get; }

		/// <summary>
		///     Gets the value of the enum member.
		/// </summary>
		public int Value { get; }
	}
}
