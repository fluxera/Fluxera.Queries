namespace Fluxera.Queries.Options
{
	using Fluxera.Queries.Model;
	using Fluxera.Queries.Nodes;
	using Fluxera.Queries.Parsers;
	using JetBrains.Annotations;

	/// <summary>
	///     A class containing de-serialized values from the $filter query option.
	/// </summary>
	[PublicAPI]
	public sealed class FilterQueryOption : QueryOption
	{
		internal FilterQueryOption(string expression, EdmComplexType edmType, EdmTypeProvider typeProvider)
			: base(expression)
		{
			this.Expression = FilterExpressionParser.Parse(expression, edmType, typeProvider);
		}

		/// <summary>
		///     Gets the expression.
		/// </summary>
		public QueryNode Expression { get; }

		/// <inheritdoc />
		public override string ToString()
		{
			string expression = this.Expression?.ToString() ?? "<none>";

			return $"Filter={expression}";
		}
	}
}
