namespace Fluxera.Queries.Model
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using Fluxera.Guards;
	using JetBrains.Annotations;

	/// <summary>
	///     The EDM representation of a complex type.
	/// </summary>
	/// <seealso cref="EdmType" />
	[PublicAPI]
	[DebuggerDisplay("{FullName}: {ClrType}")]
	public sealed class EdmComplexType : EdmType
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="EdmComplexType" /> type.
		/// </summary>
		/// <param name="clrType">The underlying CLR type.</param>
		/// <param name="properties">The EDM properties of the type.</param>
		/// <exception cref="ArgumentNullException"></exception>
		internal EdmComplexType(Type clrType, IReadOnlyList<EdmProperty> properties)
			: this(Guard.Against.Null(clrType), null, properties)
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="EdmComplexType" /> type.
		/// </summary>
		/// <param name="clrType">The underlying CLR type.</param>
		/// <param name="baseType">The (optional) base EDM type.</param>
		/// <param name="properties">The EDM properties of the type.</param>
		/// <exception cref="ArgumentNullException"></exception>
		internal EdmComplexType(Type clrType, EdmType baseType, IReadOnlyList<EdmProperty> properties)
			: base(Guard.Against.Null(clrType).Name, Guard.Against.Null(clrType).FullName, clrType)
		{
			this.BaseType = baseType;
			this.Properties = Guard.Against.Null(properties);
		}

		/// <summary>
		///     Gets the <see cref="EdmType" /> from which the current <see cref="EdmComplexType" /> directly inherits.
		/// </summary>
		public EdmType BaseType { get; }

		/// <summary>
		///     Gets the <see cref="EdmProperty" />(s) of the type.
		/// </summary>
		public IReadOnlyList<EdmProperty> Properties { get; }

		/// <summary>
		///     Gets the <see cref="EdmProperty" /> with the specified name.
		/// </summary>
		/// <param name="name">The name of the property.</param>
		/// <returns>The declared <see cref="EdmProperty" /> with the specified name.</returns>
		/// <exception cref="ArgumentException"></exception>
		public EdmProperty GetProperty(string name)
		{
			Guard.Against.Null(name);

			EdmProperty property = this.Properties.FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));

			if(property is not null)
			{
				return property;
			}

			throw new ArgumentException($"The type '{this.FullName}' does not contain a property named '{name}'");
		}
	}
}
