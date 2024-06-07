namespace Fluxera.Queries.Parsers
{
	using System.Diagnostics;

	[DebuggerDisplay("{TokenType}: {Value}")]
	internal struct Token
	{
		public Token(string value, TokenType tokenType)
		{
			this.Value = value;
			this.TokenType = tokenType;
		}

		public string Value { get; }

		public TokenType TokenType { get; }
	}
}
