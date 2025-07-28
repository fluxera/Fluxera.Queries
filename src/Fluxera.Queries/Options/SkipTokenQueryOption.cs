namespace Fluxera.Queries.Options
{
	using Fluxera.Queries.Parsers;
	using JetBrains.Annotations;

	/// <summary>
	///     A class containing de-serialized values from the $skiptoken query option.
	/// </summary>
	[PublicAPI]
	public sealed class SkipTokenQueryOption : QueryOption
	{
		/// <inheritdoc />
		public SkipTokenQueryOption(string expression)
			: base(expression)
		{
			this.SkipTokenValue = SkipTokenExpressionParser.Parse(expression);
		}

		/// <summary>
		///		Gets the skiptoken value.
		/// </summary>
		public string SkipTokenValue { get; set; }

		/// <inheritdoc />
		public override string ToString()
		{
			return $"SkipToken={this.SkipTokenValue}";
		}
	}
}
