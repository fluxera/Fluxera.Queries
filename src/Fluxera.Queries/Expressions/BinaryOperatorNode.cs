namespace Fluxera.Queries.Expressions
{
	using System.Diagnostics;
	using JetBrains.Annotations;

	/// <summary>
	///     A <see cref="QueryNode" /> which represents a binary operator with a left and right branch.
	/// </summary>
	[PublicAPI]
	[DebuggerDisplay("{Left} {OperatorKind} {Right}")]
	public sealed class BinaryOperatorNode : QueryNode
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="BinaryOperatorNode" /> type.
		/// </summary>
		/// <param name="left">The left query node.</param>
		/// <param name="operatorKind">Kind of the operator.</param>
		/// <param name="right">The right query node.</param>
		public BinaryOperatorNode(QueryNode left, BinaryOperatorKind operatorKind, QueryNode right)
		{
			this.Left = left;
			this.OperatorKind = operatorKind;
			this.Right = right;
		}

		/// <summary>
		///     Gets the kind of query node.
		/// </summary>
		public override QueryNodeKind Kind { get; } = QueryNodeKind.BinaryOperator;

		/// <summary>
		///     Gets the left query node.
		/// </summary>
		public QueryNode Left { get; set; }

		/// <summary>
		///     Gets the kind of the operator.
		/// </summary>
		public BinaryOperatorKind OperatorKind { get; }

		/// <summary>
		///     Gets the right query node.
		/// </summary>
		public QueryNode Right { get; set; }

		/// <inheritdoc />
		public override string ToString()
		{
			return $"({this.Left} {this.OperatorKind} {this.Right})";
		}
	}
}
