namespace Fluxera.Queries.Parsers
{
	internal static class CountExpressionParser
	{
		public static bool Parse(string expression)
		{
			if(!bool.TryParse(expression, out bool count))
			{
				throw new QueryException("Invalid value for $count.");
			}

			return count;
		}
	}
}