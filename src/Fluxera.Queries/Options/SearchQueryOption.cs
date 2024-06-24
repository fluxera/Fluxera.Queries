namespace Fluxera.Queries.Options
{
	using System.Linq.Expressions;
	using Fluxera.Guards;
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
		public SearchQueryOption(string expression, LambdaExpression searchPredicate)
			: base(expression)
		{
			this.SearchPredicate = Guard.Against.Null(searchPredicate);
			this.Expression = SearchExpressionParser.Parse(expression);
		}

		/// <summary>
		///		Gets the search term.
		/// </summary>
		public QueryNode Expression { get; set; }

		/// <summary>
		///		Gets the default search predicate.
		/// </summary>
		public LambdaExpression SearchPredicate { get; }

		/// <inheritdoc />
		public override string ToString()
		{
			string expression = this.Expression?.ToString() ?? "<none>";

			return $"Search={expression}";
		}
	}
}
