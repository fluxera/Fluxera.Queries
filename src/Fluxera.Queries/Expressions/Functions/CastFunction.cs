namespace Fluxera.Queries.Expressions.Functions
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class CastFunction : QueryableFunction
	{
		public override string FunctionName => "cast";

		public override Expression CreateExpression(IList<Expression> arguments)
		{
			this.ValidateParameterCount(arguments, 2);

			Type castType = TypeUtilities.ParseTargetType(arguments[1]);
			if(castType == null)
			{
				throw new InvalidOperationException("No proper type for cast specified.");
			}

			return ExpressionHelper.CreateCastExpression(arguments[0], castType);
		}
	}
}
