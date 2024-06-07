namespace Fluxera.Queries.Parsers
{
	internal static class TopExpressionParser
	{
		public static int? Parse(string expression)
		{
			return expression != null
				? int.TryParse(expression, out int topValue)
					? topValue
					: throw new QueryException("Invalid value for $top.")
				: null;
		}
	}
}