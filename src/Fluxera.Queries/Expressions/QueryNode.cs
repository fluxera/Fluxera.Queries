namespace Fluxera.Queries.Expressions
{
	using JetBrains.Annotations;

	/// <summary>
	///     The base class for a query node.
	/// </summary>
	[PublicAPI]
	public abstract class QueryNode
	{
		/// <summary>
		///     Gets the kind of query node.
		/// </summary>
		public abstract QueryNodeKind Kind { get; }

		/// <inheritdoc />
		public override string ToString()
		{
			return this.Kind.ToString();
		}
	}
}
