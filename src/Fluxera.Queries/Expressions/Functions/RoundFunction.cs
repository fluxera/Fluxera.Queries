namespace Fluxera.Queries.Expressions.Functions
{
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class RoundFunction : QueryableFunction
	{
		public override string FunctionName => "round";

		public override Expression CreateExpression(IList<Expression> arguments)
		{
			this.ValidateParameterCount(arguments, 1);

			Expression roundArg = arguments[0].Type == TypeUtilities.DoubleType
				? arguments[0]
				: Expression.Convert(arguments[0], TypeUtilities.DoubleType);
			return Expression.Call(null, Methods.MathRound, roundArg);
		}
	}
}
