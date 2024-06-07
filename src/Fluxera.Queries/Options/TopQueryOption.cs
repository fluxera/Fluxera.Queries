namespace Fluxera.Queries.Options
{
	using Fluxera.Queries.Parsers;
	using JetBrains.Annotations;

	/// <summary>
	///     A class containing de-serialized values from the '$top' query option.
	/// </summary>
	[PublicAPI]
	public sealed class TopQueryOption : QueryOption
	{
		/// <inheritdoc />
		public TopQueryOption(string expression)
			: base(expression)
		{
			this.TopValue = TopExpressionParser.Parse(expression);
		}

		/// <summary>
		///		Gets the top value.
		/// </summary>
		public int? TopValue { get; set; }

		/// <inheritdoc />
		public override string ToString()
		{
			return $"Top={this.TopValue}";
		}
	}
}
