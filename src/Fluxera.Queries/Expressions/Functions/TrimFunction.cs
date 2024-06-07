namespace Fluxera.Queries.Expressions.Functions
{
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class TrimFunction : QueryableFunction
	{
		public override string FunctionName => "trim";

		public override Expression CreateExpression(IList<Expression> arguments)
		{
			this.ValidateParameterCount(arguments, 1);

			if(arguments[0].Type != TypeUtilities.StringType)
			{
				return this.InvalidParameterTypes("strings");
			}

			return Expression.Call(arguments[0], Methods.StringTrim);
		}
	}
}
