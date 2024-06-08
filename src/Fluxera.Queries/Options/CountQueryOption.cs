namespace Fluxera.Queries.Options
{
	using Fluxera.Queries.Parsers;
	using JetBrains.Annotations;

	/// <summary>
	///     A class containing de-serialized values from the $count query option.
	/// </summary>
	[PublicAPI]
	public sealed class CountQueryOption : QueryOption
	{
		/// <inheritdoc />
		public CountQueryOption(string expression)
			: base(expression)
		{
			this.CountValue = CountExpressionParser.Parse(expression);
		}

		/// <summary>
		///		Gets the count value.
		/// </summary>
		public bool CountValue { get; set; }

		/// <inheritdoc />
		public override string ToString()
		{
			return $"Count={this.CountValue}";
		}
	}
}
