namespace Fluxera.Queries.Options
{
	using Fluxera.Queries.Nodes;
	using Fluxera.Queries.Parsers;
	using JetBrains.Annotations;

	/// <summary>
	///     A class containing de-serialized values from the $search query option.
	/// </summary>
	[PublicAPI]
	public sealed class SearchQueryOption : QueryOption
	{
		/// <inheritdoc />
		public SearchQueryOption(string expression)
			: base(expression)
		{
			this.Expression = SearchExpressionParser.Parse(expression);
		}

		/// <summary>
		///		Gets the search term.
		/// </summary>
		public QueryNode Expression { get; set; }

		/// <inheritdoc />
		public override string ToString()
		{
			string expression = this.Expression?.ToString() ?? "<none>";

			return $"Search={expression}";
		}
	}
}
