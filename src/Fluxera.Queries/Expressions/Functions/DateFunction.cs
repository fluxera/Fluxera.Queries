namespace Fluxera.Queries.Expressions.Functions
{
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class DateFunction : QueryableFunction
	{
		public override string FunctionName => "date";

		public override Expression CreateExpression(IList<Expression> arguments)
		{
			this.ValidateParameterCount(arguments, 1);

			if(arguments[0].Type == TypeUtilities.DateTimeType)
			{
				return Expression.MakeMemberAccess(arguments[0], Methods.DateTimeDate);
			}

			if(arguments[0].Type == TypeUtilities.DateTimeOffsetType)
			{
				return Expression.MakeMemberAccess(arguments[0], Methods.DateTimeOffsetDate);
			}

			return this.InvalidParameterTypes("DateTime, DateTimeOffset");
		}
	}
}