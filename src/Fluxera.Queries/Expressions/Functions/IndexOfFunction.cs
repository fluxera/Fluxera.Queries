namespace Fluxera.Queries.Expressions.Functions
{
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class IndexOfFunction : QueryableFunction
	{
		public override string FunctionName => "indexof";

		public override Expression CreateExpression(IList<Expression> arguments)
		{
			this.ValidateParameterCount(arguments, 2);
			return Expression.Call(arguments[0], Methods.StringIndexOf, arguments[1]);
		}
	}
}