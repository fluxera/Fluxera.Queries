namespace Fluxera.Queries.Options
{
	using Fluxera.Queries.Parsers;
	using JetBrains.Annotations;

	/// <summary>
	///     A class containing de-serialized values from the '$skip' query option.
	/// </summary>
	[PublicAPI]
	public sealed class SkipQueryOption : QueryOption
	{
		/// <inheritdoc />
		public SkipQueryOption(string expression)
			: base(expression)
		{
			this.SkipValue = SkipExpressionParser.Parse(expression);
		}

		/// <summary>
		///		Gets the skip value.
		/// </summary>
		public int? SkipValue { get; set; }

		/// <inheritdoc />
		public override string ToString()
		{
			return $"Skip={this.SkipValue}";
		}
	}
}
