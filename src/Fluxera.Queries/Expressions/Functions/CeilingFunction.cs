namespace Fluxera.Queries.Expressions.Functions
{
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class CeilingFunction : QueryableFunction
	{
		public override string FunctionName => "ceiling";

		public override Expression CreateExpression(IList<Expression> arguments)
		{
			this.ValidateParameterCount(arguments, 1);

			Expression ceilArg = arguments[0].Type == TypeUtilities.DoubleType
				? arguments[0]
				: Expression.Convert(arguments[0], TypeUtilities.DoubleType);

			return Expression.Call(null, Methods.MathCeiling, ceilArg);
		}
	}
}