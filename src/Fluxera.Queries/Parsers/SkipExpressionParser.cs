namespace Fluxera.Queries.Parsers
{
	internal static class SkipExpressionParser
	{
		public static int? Parse(string expression)
		{
			return expression != null
				? int.TryParse(expression, out int skipValue)
					? skipValue
					: throw new QueryException("Invalid value for $skip.")
				: null;
		}
	}
}