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
	///     A class containing de-serialized values from the '$orderby' query option.
	/// </summary>
	[PublicAPI]
	public sealed class OrderByProperty
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="OrderByProperty" /> type.
		/// </summary>
		/// <param name="property">The EDM property to order by.</param>
		/// <param name="direction">The order direction.</param>
		public OrderByProperty(EdmProperty property, OrderByDirection direction)
			: this([property], direction)
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="OrderByProperty" /> type.
		/// </summary>
		/// <param name="properties"></param>
		/// <param name="direction"></param>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		public OrderByProperty(IEnumerable<EdmProperty> properties, OrderByDirection direction)
		{
			properties = Guard.Against.Null(properties);

			this.Properties = new ReadOnlyCollection<EdmProperty>(properties.ToList());

			if(this.Properties.Count == 0)
			{
				throw new InvalidOperationException("At least one order by property must be selected.");
			}

			this.Direction = direction;
		}

		/// <summary>
		///     Gets the direction the property should be ordered by.
		/// </summary>
		public OrderByDirection Direction { get; }

		/// <summary>
		///     Gets the properties to order by.
		/// </summary>
		public IReadOnlyList<EdmProperty> Properties { get; }

		/// <inheritdoc />
		public override string ToString()
		{
			return $"{string.Join(".", this.Properties)}-{this.Direction}";
		}
	}
}
