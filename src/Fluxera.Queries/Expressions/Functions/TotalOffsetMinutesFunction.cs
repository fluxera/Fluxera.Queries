namespace Fluxera.Queries.Expressions.Functions
{
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class TotalOffsetMinutesFunction : QueryableFunction
	{
		public override string FunctionName => "totaloffsetminutes";

		public override Expression CreateExpression(IList<Expression> arguments)
		{
			this.ValidateParameterCount(arguments, 1);

			if(arguments[0].Type != TypeUtilities.DateTimeOffsetType)
			{
				return this.InvalidParameterTypes("DateTimeOffset");
			}

			return Expression.MakeMemberAccess(
				Expression.MakeMemberAccess(arguments[0], Methods.DateTimeOffsetOffset),
				Methods.TimeSpanTotalMinutes);
		}
	}
}
