namespace Fluxera.Queries.Expressions
{
	using Fluxera.Queries.Model;
	using JetBrains.Annotations;

	/// <summary>
	///     A <see cref="QueryNode" /> which represents a value (property or constant).
	/// </summary>
	[PublicAPI]
	public abstract class ValueNode : QueryNode
	{
		/// <summary>
		///     Gets the <see cref="EdmType" /> of the value node.
		/// </summary>
		public abstract EdmType EdmValueType { get; }

		/// <inheritdoc />
		public override string ToString()
		{
			return this.EdmValueType?.Name;
		}
	}
}
