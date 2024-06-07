namespace Fluxera.Queries.Expressions.Functions
{
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class EndsWithFunction : QueryableFunction
	{
		public override string FunctionName => "endswith";

		public override Expression CreateExpression(IList<Expression> arguments)
		{
			this.ValidateParameterCount(arguments, 2);
			return Expression.Call(arguments[0], Methods.StringEndsWith, arguments[1]);
		}
	}
}
