namespace Fluxera.Queries.Expressions
{
	using System.Diagnostics;
	using JetBrains.Annotations;

	/// <summary>
	///     A <see cref="QueryNode" /> which represents a unary operator.
	/// </summary>
	[PublicAPI]
	[DebuggerDisplay("{OperatorKind} {Operand}")]
	public sealed class UnaryOperatorNode : QueryNode
	{
		/// <summary>
		///     Initialises a new instance of the <see cref="UnaryOperatorNode" /> class.
		/// </summary>
		/// <param name="operand">The operand of the unary operator.</param>
		/// <param name="operatorKind">Kind of the operator.</param>
		public UnaryOperatorNode(QueryNode operand, UnaryOperatorKind operatorKind)
		{
			this.Operand = operand;
			this.OperatorKind = operatorKind;
		}

		/// <summary>
		///     Gets the kind of query node.
		/// </summary>
		public override QueryNodeKind Kind => QueryNodeKind.UnaryOperator;

		/// <summary>
		///     Gets the operand of the unary operator.
		/// </summary>
		public QueryNode Operand { get; }

		/// <summary>
		///     Gets the kind of the operator.
		/// </summary>
		public UnaryOperatorKind OperatorKind { get; }

		/// <inheritdoc />
		public override string ToString()
		{
			return $"{this.OperatorKind} {this.Operand}";
		}
	}
}
