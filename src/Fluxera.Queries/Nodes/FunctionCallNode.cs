namespace Fluxera.Queries.Nodes
{
	using System.Collections.Generic;
	using System.Diagnostics;
	using Fluxera.Guards;
	using JetBrains.Annotations;

	/// <summary>
	///     A <see cref="QueryNode" /> which represents a function call.
	/// </summary>
	[PublicAPI]
	[DebuggerDisplay("{Name}")]
	public sealed class FunctionCallNode : QueryNode
	{
		private readonly List<QueryNode> parameters = new List<QueryNode>();

		/// <summary>
		///     Initializes a new instance of the <see cref="FunctionCallNode" /> type.
		/// </summary>
		/// <param name="name">The name of the function.</param>
		public FunctionCallNode(string name)
		{
			this.Name = Guard.Against.NullOrWhiteSpace(name);
		}

		/// <summary>
		///     Gets the kind of query node.
		/// </summary>
		public override QueryNodeKind Kind => QueryNodeKind.FunctionCall;

		/// <summary>
		///     Gets the name of the function.
		/// </summary>
		public string Name { get; }

		/// <summary>
		///     Gets the parameters for the function call.
		/// </summary>
		public IReadOnlyList<QueryNode> Parameters => this.parameters;

		/// <summary>
		///		Adds a parameter node to this function call node.
		/// </summary>
		/// <param name="queryNode">The node to add.</param>
		public void AddParameter(QueryNode queryNode)
		{
			this.parameters.Add(queryNode);
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return $"{this.Name}({string.Join(",", this.Parameters)})";
		}
	}
}
