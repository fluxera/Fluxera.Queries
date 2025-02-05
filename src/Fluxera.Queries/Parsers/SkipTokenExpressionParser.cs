namespace Fluxera.Queries.Parsers
{
	internal static class SkipTokenExpressionParser
	{
		public static string Parse(string expression)
		{
			if(string.IsNullOrWhiteSpace(expression))
			{
				throw new QueryException("Invalid value for $skiptoken.");
			}

			return expression;
		}
	}
}
