namespace Fluxera.Queries.Expressions.Functions
{
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class StartsWithFunction : QueryableFunction
	{
		public override string FunctionName => "startswith";

		public override Expression CreateExpression(IList<Expression> arguments)
		{
			this.ValidateParameterCount(arguments, 2);
			return Expression.Call(arguments[0], Methods.StringStartsWith, arguments[1]);
		}
	}
}