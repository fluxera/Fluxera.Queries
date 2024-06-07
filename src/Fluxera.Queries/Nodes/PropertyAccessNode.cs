namespace Fluxera.Queries.Nodes
{
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Diagnostics;
	using System.Linq;
	using Fluxera.Queries.Model;
	using JetBrains.Annotations;

	/// <summary>
	///     A <see cref="QueryNode" /> which represents a property.
	/// </summary>
	[PublicAPI]
	[DebuggerDisplay("{Properties}")]
	public sealed class PropertyAccessNode : ValueNode
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="PropertyAccessNode" /> type.
		/// </summary>
		/// <param name="properties">The property being referenced in the query.</param>
		public PropertyAccessNode(IEnumerable<EdmProperty> properties)
		{
			this.Properties = new ReadOnlyCollection<EdmProperty>(properties.ToList());
		}

		/// <summary>
		///     Gets the kind of query node.
		/// </summary>
		public override QueryNodeKind Kind => QueryNodeKind.PropertyAccess;

		/// <summary>
		///     Gets the properties being referenced in the query.
		/// </summary>
		public IReadOnlyList<EdmProperty> Properties { get; }

		/// <summary>
		///     Gets the <see cref="EdmType" /> of the property value.
		/// </summary>
		public override EdmType EdmValueType => this.Properties.Last().PropertyType;

		/// <inheritdoc />
		public override string ToString()
		{
			return string.Join(".", this.Properties);
		}
	}
}
