namespace Fluxera.Queries.Expressions.Functions
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class LengthFunction : QueryableFunction
	{
		public override string FunctionName => "length";

		public override Expression CreateExpression(IList<Expression> arguments)
		{
			this.ValidateParameterCount(arguments, 1);

			if(arguments[0].Type == TypeUtilities.StringType)
			{
				return Expression.MakeMemberAccess(arguments[0], Methods.StringLength);
			}

			if(ReflectionHelper.IsEnumerable(arguments[0].Type, out Type itemType))
			{
				return Expression.Call(null, Methods.EnumerableCount.MakeGenericMethod(itemType), arguments[0]);
			}

			return this.InvalidParameterTypes("strings, enumerables");
		}
	}
}