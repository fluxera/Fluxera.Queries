namespace Fluxera.Queries.Expressions.Functions
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class IsOfFunction : QueryableFunction
	{
		public override string FunctionName => "isof";

		public override Expression CreateExpression(IList<Expression> arguments)
		{
			this.ValidateParameterCount(arguments, 2);

			Type typeCheckType = TypeUtilities.ParseTargetType(arguments[1]);
			if(typeCheckType == null)
			{
				throw new InvalidOperationException("No proper type for type check specified.");
			}

			return Expression.TypeIs(arguments[0], typeCheckType);
		}
	}
}
