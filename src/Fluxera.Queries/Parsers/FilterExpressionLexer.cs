namespace Fluxera.Queries.Parsers
{
	using System;
	using System.Text.RegularExpressions;

	internal struct FilterExpressionLexer
	{
		// More restrictive expressions should be added before less restrictive expressions which could also match.
		// Also, within those bounds then order by the most common first where possible.
		private static readonly TokenDefinition[] TokenDefinitions =
		[
			new TokenDefinition(TokenType.OpenParentheses, @"\("),
			new TokenDefinition(TokenType.CloseParentheses, @"\)"),
			new TokenDefinition(TokenType.And, @"and(?=\s|$)"),
			new TokenDefinition(TokenType.Or, @"or(?=\s|$)"),
			new TokenDefinition(TokenType.True, @"true"),
			new TokenDefinition(TokenType.False, @"false"),
			new TokenDefinition(TokenType.Null, @"null"),
			new TokenDefinition(TokenType.UnaryOperator, @"not(?=\s|$)"),
			new TokenDefinition(TokenType.BinaryOperator, @"(eq|ne|gt|ge|lt|le|has|add|sub|mul|div|mod|in)(?=\s|$)"),
			new TokenDefinition(TokenType.DateTimeOffset, @"\d{4}-\d{2}-\d{2}T\d{2}:\d{2}(:\d{2}(\.\d{1,12})?(Z|[+-]\d{2}:\d{2})?)?"),
			new TokenDefinition(TokenType.Date, @"\d{4}-\d{2}-\d{2}"),
			new TokenDefinition(TokenType.TimeOfDay, @"\d{2}:\d{2}(:\d{2}(\.\d{1,12})?)?"),
			new TokenDefinition(TokenType.Guid, @"[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}"),
			new TokenDefinition(TokenType.Decimal, @"(\+|-)?(\d+(\.\d+)?|(\.\d+))(m|M)"),
			new TokenDefinition(TokenType.Single, @"(\+|-)?\d+(\.\d+)?(f|F)"),
			new TokenDefinition(TokenType.Double, @"(\+|-)?(\d+(\.\d+)(d|D)?)"),
			new TokenDefinition(TokenType.Double, @"(\+|-)?(\d+\.\d+(e|E)\d+)"),
			new TokenDefinition(TokenType.Integer, @"(\+|-)?\d+(l|L)?"),
			new TokenDefinition(TokenType.FunctionName, @"\w+(?=\()"),
			new TokenDefinition(TokenType.Comma, @",(?=\s?)"),
			new TokenDefinition(TokenType.Duration, @"duration'(-)?P\d+DT\d{2}H\d{2}M\d{2}\.\d+S'"),
			new TokenDefinition(TokenType.Enum, @"\w+(\.\w+)+'\w+(\,\w+)*'"),
			new TokenDefinition(TokenType.PropertyName, @"[\w\/]+"),
			new TokenDefinition(TokenType.String, @"'(?:''|[\w\s-.~!$&()*+,;=@%\\\/]*)*'"),
			new TokenDefinition(TokenType.Whitespace, @"\s", true)
		];

		private readonly string content;
		private int position;

		public FilterExpressionLexer(string content)
		{
			this.content = content;
			this.Current = default;

			this.position = this.content.StartsWith("$filter=", StringComparison.Ordinal)
				? content.IndexOf('=') + 1
				: 0;
		}

		public Token Current { get; private set; }

		public bool MoveNext()
		{
			if (this.content.Length == this.position)
			{
				return false;
			}

			for (int i = 0; i < TokenDefinitions.Length; i++)
			{
				TokenDefinition tokenDefinition = TokenDefinitions[i];

				Match match = tokenDefinition.Regex.Match(this.content, this.position);

				if (match.Success)
				{
					if (tokenDefinition.Ignore)
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

			if (this.content.Length != this.position)
			{
				throw new QueryException(Messages.UnableToParseFilter);
			}

			return false;
		}
	}
}
