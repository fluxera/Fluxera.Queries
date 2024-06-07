namespace Fluxera.Queries.Parsers
{
	using Fluxera.Queries.Expressions;

	internal static class UnaryOperatorKindParser
	{
		public static UnaryOperatorKind ToUnaryOperatorKind(this string operatorType)
		{
			return operatorType switch
			{
				"not" => UnaryOperatorKind.Not,
				_     => throw new QueryException(string.Format(Messages.UnknownOperator, operatorType))
			};
		}
	}
}
