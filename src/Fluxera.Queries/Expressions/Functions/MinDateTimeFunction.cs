namespace Fluxera.Queries.Expressions.Functions
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class MinDateTimeFunction : QueryableFunction
	{
		public override string FunctionName => "mindatetime";

		public override Expression CreateExpression(IList<Expression> arguments)
		{
			this.ValidateParameterCount(arguments, 0);
			return Expression.Constant(DateTimeOffset.MinValue);
		}
	}
}