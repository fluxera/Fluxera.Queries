namespace Fluxera.Queries.Expressions.Functions
{
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class MonthFunction : QueryableFunction
	{
		public override string FunctionName => "month";

		public override Expression CreateExpression(IList<Expression> arguments)
		{
			this.ValidateParameterCount(arguments, 1);

			if(arguments[0].Type == TypeUtilities.DateOnlyType)
			{
				return Expression.MakeMemberAccess(arguments[0], Methods.DateOnlyMonth);
			}

			if(arguments[0].Type == TypeUtilities.DateTimeType)
			{
				return Expression.MakeMemberAccess(arguments[0], Methods.DateTimeMonth);
			}

			if(arguments[0].Type == TypeUtilities.DateTimeOffsetType)
			{
				return Expression.MakeMemberAccess(arguments[0], Methods.DateTimeOffsetMonth);
			}

			return this.InvalidParameterTypes("DateTime, DateTimeOffset");
		}
	}
}