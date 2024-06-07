namespace Fluxera.Queries.Model
{
	using System;
	using JetBrains.Annotations;

	/// <summary>
	///     The EDM representation of an entity set.
	/// </summary>
	[PublicAPI]
	public sealed class EntitySet
	{
		/// <summary>
		///		Initializes a new instance of the <see cref="EntitySet"/> type.
		/// </summary>
		/// <param name="name">The name of the entity set.</param>
		/// <param name="entityType">The EDM type of the entities in the set.</param>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		internal EntitySet(string name, EdmComplexType entityType)
		{
			this.Name = Guard.ThrowIfNullOrWhiteSpace(name);
			this.EdmType = Guard.ThrowIfNull(entityType);
		}

		/// <summary>
		///		Gets the name of the <see cref="EntitySet"/>.
		/// </summary>
		public string Name { get; }

		/// <summary>
		///		Gets the <see cref="EdmComplexType"/> of the entities in the <see cref="EntitySet"/>.
		/// </summary>
		public EdmComplexType EdmType { get; }
	}
}
