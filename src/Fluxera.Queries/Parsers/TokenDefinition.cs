namespace Fluxera.Queries.Parsers
{
	using System.Diagnostics;
	using System.Text.RegularExpressions;

	[DebuggerDisplay("{tokenType}: {Regex}")]
	internal readonly struct TokenDefinition
	{
		private readonly TokenType tokenType;

		public TokenDefinition(TokenType tokenType, string expression, bool ignore = false)
		{
			this.tokenType = tokenType;
			this.Regex = new Regex(@"\G" + expression, RegexOptions.Singleline | RegexOptions.Compiled);
			this.Ignore = ignore;
		}

		public Regex Regex { get; }

		public bool Ignore { get; }

		public Token CreateToken(Match match)
		{
			return new Token(match.Value, this.tokenType);
		}
	}
}
