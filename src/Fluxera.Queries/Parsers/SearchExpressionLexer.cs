namespace Fluxera.Queries.Parsers
{
	using System;
	using System.Text.RegularExpressions;

	internal struct SearchExpressionLexer
	{
		// More restrictive expressions should be added before less restrictive expressions which could also match.
		// Also, within those bounds then order by the most common first where possible.
		private static readonly TokenDefinition[] TokenDefinitions =
		[
			new TokenDefinition(TokenType.OpenParentheses, @"\("),
			new TokenDefinition(TokenType.CloseParentheses, @"\)"),
			new TokenDefinition(TokenType.And, @"AND(?=\s)"),
			new TokenDefinition(TokenType.Or, @"OR(?=\s)"),
			new TokenDefinition(TokenType.UnaryOperator, @"NOT(?=\s)"),
			new TokenDefinition(TokenType.String, @"""(?:''|[\w\s-.~!$&()*+,;=@%\\\/]*)*"""),
			new TokenDefinition(TokenType.String, @"[\w\/]+"),
			new TokenDefinition(TokenType.Whitespace, @"\s", true)
		];

		private readonly string content;
		private int position;

		public SearchExpressionLexer(string content)
		{
			this.content = content;
			this.Current = default;

			this.position = this.content.StartsWith("$search=", StringComparison.Ordinal)
				? content.IndexOf('=') + 1
				: 0;
		}

		public Token Current { get; private set; }

		public bool MoveNext()
		{
			if(this.content.Length == this.position)
			{
				return false;
			}

			for(int i = 0; i < TokenDefinitions.Length; i++)
			{
				TokenDefinition tokenDefinition = TokenDefinitions[i];

				Match match = tokenDefinition.Regex.Match(this.content, this.position);

				if(match.Success)
				{
					if(tokenDefinition.Ignore)
					{
						this.position += match.Length;
						i = -1;
						continue;
					}

					this.Current = tokenDefinition.CreateToken(match);
					this.position += match.Length;

					return true;
				}
			}

			if(this.content.Length != this.position)
			{
				throw new QueryException(Messages.UnableToParseSearch);
			}

			return false;
		}
	}
}
