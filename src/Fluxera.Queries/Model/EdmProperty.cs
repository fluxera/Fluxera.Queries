namespace Fluxera.Queries.Model
{
	using System;
	using System.Diagnostics;
	using Fluxera.Guards;
	using JetBrains.Annotations;

	/// <summary>
	///     The EDM representation of a property of an <see cref="EdmComplexType"/>.
	/// </summary>
	[PublicAPI]
	[DebuggerDisplay("{Name}")]
	public sealed class EdmProperty
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="EdmProperty" /> type.
		/// </summary>
		/// <param name="name">The name of the property.</param>
		/// <param name="propertyType">The EDM type of the property.</param>
		/// <param name="declaringType">The declaring EDM type of the property.</param>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		internal EdmProperty(string name, EdmType propertyType, EdmComplexType declaringType)
		{
			this.Name = Guard.Against.NullOrWhiteSpace(name);
			this.PropertyType = Guard.Against.Null(propertyType);
			this.DeclaringType = Guard.Against.Null(declaringType);
		}

		/// <summary>
		///     Gets the <see cref="EdmComplexType" /> which declares this property.
		/// </summary>
		public EdmComplexType DeclaringType { get; }

		/// <summary>
		///     Gets the name of the property.
		/// </summary>
		public string Name { get; }

		/// <summary>
		///     Gets the <see cref="EdmType" /> of the property.
		/// </summary>
		public EdmType PropertyType { get; }

		/// <inheritdoc />
		public override string ToString()
		{
			return this.Name;
		}
	}
}
