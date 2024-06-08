namespace Fluxera.Queries.Options
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using Fluxera.Guards;
	using Fluxera.Queries.Model;
	using JetBrains.Annotations;

	/// <summary>
	///     A class containing de-serialized values from the $select query option.
	/// </summary>
	[PublicAPI]
	public sealed class SelectProperty
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="SelectProperty" /> type.
		/// </summary>
		/// <param name="property">The EDM property to select.</param>
		public SelectProperty(EdmProperty property)
			: this([property])
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="SelectProperty" /> type.
		/// </summary>
		/// <param name="properties"></param>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		public SelectProperty(IEnumerable<EdmProperty> properties)
		{
			properties = Guard.Against.Null(properties);

			this.Properties = new ReadOnlyCollection<EdmProperty>(properties.ToList());

			if(this.Properties.Count == 0)
			{
				throw new InvalidOperationException("At least one select property must be selected.");
			}
		}

		/// <summary>
		///     Gets the properties to select.
		/// </summary>
		public IReadOnlyList<EdmProperty> Properties { get; }

		/// <inheritdoc />
		public override string ToString()
		{
			return $"{string.Join(".", this.Properties)}";
		}
	}
}
