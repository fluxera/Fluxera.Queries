namespace Fluxera.Queries.Expressions.Functions
{
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class DayFunction : QueryableFunction
	{
		public override string FunctionName => "day";

		public override Expression CreateExpression(IList<Expression> arguments)
		{
			this.ValidateParameterCount(arguments, 1);

			if(arguments[0].Type == TypeUtilities.DateOnlyType)
			{
				return Expression.MakeMemberAccess(arguments[0], Methods.DateOnlyDay);
			}

			if(arguments[0].Type == TypeUtilities.DateTimeType)
			{
				return Expression.MakeMemberAccess(arguments[0], Methods.DateTimeDay);
			}

			if(arguments[0].Type == TypeUtilities.DateTimeOffsetType)
			{
				return Expression.MakeMemberAccess(arguments[0], Methods.DateTimeOffsetDay);
			}

			return this.InvalidParameterTypes("DateOnly, DateTime, DateTimeOffset");
		}
	}
}
